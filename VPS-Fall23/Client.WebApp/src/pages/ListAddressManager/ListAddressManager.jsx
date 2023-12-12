import { AutoComplete, Pagination, Select, Table, Tag, Tooltip } from 'antd';
import { CloseSquareFilled } from '@ant-design/icons';
import React, { Fragment, useEffect, useState } from 'react';
import useAddressServices from '@/services/addressServices';
import CreateAddress from '@/pages/ListAddressManager/components/CreateAddress.jsx';
import optionsCreateAddressType from '@/helpers/optionsCreateAddressType.js';
import addressTypeEnum from '../../helpers/addressTypeEnum.js';

function ListAddressManager() {

  const columnsTableAddress = {
    commune: [
      {
        title: 'Mã vùng',
        dataIndex: 'communeCode',
        key: 'communeCode',
      },
      {
        title: 'Tỉnh/Thành phố',
        dataIndex: 'cityName',
        key: 'cityName',
      },
      {
        title: 'Quận/Huyện',
        dataIndex: 'districtName',
        key: 'districtName',
      },
      {
        title: 'Phường/Xã',
        dataIndex: 'communeName',
        key: 'communeName',
      },
      {
        title: 'Ẩn/Hiện',
        key: 'isBlock',
        render: (val) => (
          <Fragment>
            {val.isBlock === false ?
              (
                <Tooltip title='Nhấn để ẩn địa điểm'>
                  <Tag onClick={() => ChangeIsBlock(val.isBlock, val.communeId)} color='success'>
                    <a>Hiện thị</a>
                  </Tag>
                </Tooltip>
              ) :
              (
                <Tooltip title='Nhấn để hiện địa điểm'>
                  <Tag onClick={() => ChangeIsBlock(val.isBlock, val.communeId)} color='red'>
                    <a>Ẩn</a>
                  </Tag>
                </Tooltip>
              )
            }
          </Fragment>
        ),
      },
    ],
    district: [
      {
        title: 'Mã vùng',
        dataIndex: 'districtCode',
        key: 'districtCode',
      },
      {
        title: 'Tỉnh/Thành phố',
        dataIndex: 'cityName',
        key: 'cityName',
      },
      {
        title: 'Quận/Huyện',
        dataIndex: 'districtName',
        key: 'districtName',
      },
      {
        title: 'Ẩn/Hiện',
        key: 'isBlock',
        render: (val) => (
          <Fragment>
            {val.isBlock === false ?
              (
                <Tooltip title='Nhấn để ẩn địa điểm'>
                  <Tag onClick={() => ChangeIsBlock(val.isBlock, val.districtId)} color='success'>
                    <a>Hiện thị</a>
                  </Tag>
                </Tooltip>
              ) :
              (
                <Tooltip title='Nhấn để hiện địa điểm'>
                  <Tag onClick={() => ChangeIsBlock(val.isBlock, val.districtId)} color='red'>
                    <a>Ẩn</a>
                  </Tag>
                </Tooltip>
              )
            }
          </Fragment>
        ),
      },
    ],
    city: [
      {
        title: 'Mã vùng',
        dataIndex: 'cityCode',
        key: 'cityCode',
      },
      {
        title: 'Tỉnh/Thành phố',
        dataIndex: 'cityName',
        key: 'cityName',
      },
      {
        title: 'Ẩn/Hiện',
        key: 'isBlock',
        render: (val) => (
          <Fragment>
            {val.isBlock === false ?
              (
                <Tooltip title='Nhấn để ẩn địa điểm'>
                  <Tag onClick={() => ChangeIsBlock(val.isBlock, val.cityId)} color='success'>
                    <a>Hiện thị</a>
                  </Tag>
                </Tooltip>
              ) :
              (
                <Tooltip title='Nhấn để hiện địa điểm'>
                  <Tag onClick={() => ChangeIsBlock(val.isBlock, val.cityId)} color='red'>
                    <a>Ẩn</a>
                  </Tag>
                </Tooltip>
              )
            }
          </Fragment>
        ),
      },
    ],
  };

  const service = useAddressServices();
  const [data, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [textSearch, setTextSearch] = useState('');
  const [cityFilter, setCityFilter] = useState(undefined);
  const [districtFilter, setDistrictFilter] = useState(undefined);
  const [typeCityOptions, setTypeCityOptions] = useState([]);
  const [typeDistrictOptions, setTypeDistrictOptions] = useState([]);
  let options = optionsCreateAddressType;
  const [addressType, setAddressType] = useState(options[2].value);

  const pageSize = 10;

  const loadData = () => {
    service.getAddressManager(pageNumber, pageSize, cityFilter, districtFilter, textSearch, addressType).then((res) => {
      setData(res.data.listAddress);
      setTotalItems(res.data.totalPages);
    });
  };

  const loadDataOnChange = (addressTypeLoad) => {
    service.getAddressManager(pageNumber, pageSize, undefined, undefined, '', addressTypeLoad).then((res) => {
      setData(res.data.listAddress);
      setTotalItems(res.data.totalPages);
    });
  };

  const handleChangePage = (page) => {
    setPageNumber(page);
  };

  const ChangeIsBlock = (isBlock, communeId) => {
    let newData = [];
    switch (addressType) {
      case addressTypeEnum.COMMUNE:
        newData = data.map((val) => ({
          ...val,
          isBlock: val.communeId === communeId ? !isBlock : val.isBlock,
        }));
        break;
      case addressTypeEnum.DISTRICT:
        newData = data.map((val) => ({
          ...val,
          isBlock: val.districtId === communeId ? !isBlock : val.isBlock,
        }));
        break;
      case addressTypeEnum.CITY:
        newData = data.map((val) => ({
          ...val,
          isBlock: val.cityId === communeId ? !isBlock : val.isBlock,
        }));
        break;
    }
    service.updateIsBlockAddress(!isBlock, communeId, addressType).then((res) => {
      setData(newData);
    });
  };
  const OnSearchAddress = (event) => {
    if (event.key === 'Enter') {
      setPageNumber(1);
      loadData();
    }
  };

  const OnChangeCityFilter = (val) => {
    setCityFilter(val);
    setDistrictFilter(undefined);
    setPageNumber(1);
    OnLoadDistrictFilter(val);
  };

  const OnChangeDistrictFilter = (val) => {
    setPageNumber(1);
    setDistrictFilter(val);
  };

  const OnLoadDistrictFilter = (val) => {
    if (val === undefined) {
      setTypeDistrictOptions([]);
    } else {
      service.getDistricts(val).then((res) => {
        setTypeDistrictOptions(res.data.map((val) => {
            return {
              value: val.id,
              label: val.name,
            };
          }),
        );
        setDistrictFilter(undefined);
      });
    }
  };

  const OnFilterOption = (input, option) => (option?.label ?? '').toLowerCase().includes(input.toLowerCase());

  const loadTypeCityOptions = () => {
    service.getCities().then((res) => {
      setTypeCityOptions(res.data.map((val) => {
          return {
            value: val.id,
            label: val.name,
          };
        }),
      );
    });
  };

  const callBackCreateAddress = () => {
    setPageNumber(1);
    setTextSearch('');
    setCityFilter(undefined);
    setDistrictFilter(undefined);
  };

  const onChangeAddressType = (val) => {
    setAddressType(val);
    setDistrictFilter(undefined);
    setCityFilter(undefined);
    setTextSearch('');
    setPageNumber(1);
    loadDataOnChange(val);
  };
  useEffect(() => {
    loadData();
    loadTypeCityOptions();
  }, [pageNumber, districtFilter, cityFilter]);

  return (
    <>
      <div className='w-[100%]'>
        <div className='mb-[16px] flex items-center justify-between'>
          <CreateAddress callBackCreateAddress={callBackCreateAddress} />
          <Select
            className={('min-w-[230px]')}
            defaultValue={options[2]}
            style={{ width: 120 }}
            options={options}
            onChange={onChangeAddressType}
          />
        </div>
        <div className='mb-[16px] flex items-center justify-between'>
          <AutoComplete
            placeholder='Tìm kiếm tên địa điểm (enter để tìm kiếm)'
            className='mt-4 mb-4 w-[300px]'
            onSearch={(val) => setTextSearch(val)}
            allowClear={{ clearIcon: <CloseSquareFilled /> }}
            value={textSearch}
            onKeyDown={OnSearchAddress} />
          <div className={'ttt mr-[10px]'}>
            {addressType === addressTypeEnum.COMMUNE || addressType === addressTypeEnum.DISTRICT ?
              <Select
                className='w-80 mr-[10px]'
                showSearch
                placeholder='Tỉnh/Thành phố'
                optionFilterProp='children'
                allowClear
                onChange={OnChangeCityFilter}
                filterOption={OnFilterOption}
                options={typeCityOptions}
                value={cityFilter}
              />
              : <></>
            }
            {addressType === addressTypeEnum.COMMUNE ?
              <Select
                className='w-80'
                showSearch
                placeholder='Quận/Huyện'
                optionFilterProp='children'
                allowClear
                onChange={OnChangeDistrictFilter}
                filterOption={OnFilterOption}
                options={typeDistrictOptions}
                value={districtFilter}
              />
              : <></>
            }
          </div>
        </div>

        {addressType === addressTypeEnum.COMMUNE ?
          <Table columns={columnsTableAddress.commune} dataSource={data} pagination={false} /> : <></>}
        {addressType === addressTypeEnum.DISTRICT ?
          <Table columns={columnsTableAddress.district} dataSource={data} pagination={false} /> : <></>}
        {addressType === addressTypeEnum.CITY ?
          <Table columns={columnsTableAddress.city} dataSource={data} pagination={false} /> : <></>}

        <div className='py-[16px] flex flex-row-reverse pr-[24px]'>
          <Pagination current={pageNumber} pageSize={pageSize} onChange={handleChangePage} total={totalItems}
                      showSizeChanger={false} />
        </div>
      </div>
    </>
  );
}

export default ListAddressManager;