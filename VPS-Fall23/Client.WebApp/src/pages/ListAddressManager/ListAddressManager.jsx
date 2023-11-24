import { AutoComplete, Pagination, Select, Table, Tag, Tooltip } from 'antd';
import { CloseSquareFilled } from '@ant-design/icons';
import React, { Fragment, useEffect, useState } from 'react';
import useAddressServices from '@/services/addressServices';

function ListAddressManager() {
  const service = useAddressServices();
  const [data, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [textSearch, setTextSearch] = useState('');
  const [cityFilter, setCityFilter] = useState(undefined);
  const [districtFilter, setDistrictFilter] = useState(undefined);
  const [typeCityOptions, setTypeCityOptions] = useState([]);
  const [typeDistrictOptions, setTypeDistrictOptions] = useState([]);
  const pageSize = 10;

  const columns = [
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
                <Tag onClick={() => ChangeIsBlock(val.communeId)} color='success'>
                  <a>Hiện thị</a>
                </Tag>
              </Tooltip>
            ) :
            (
              <Tooltip title='Nhấn để hiện địa điểm'>
                <Tag onClick={() => ChangeIsBlock(val.communeId)} color='red'>
                  <a>Ẩn</a>
                </Tag>
              </Tooltip>
            )
          }
        </Fragment>
      ),
    },
  ];

  const loadData = () => {
    service.getAddressManager(pageNumber, pageSize, cityFilter, districtFilter, textSearch).then((res) => {
      setData(res.data.listAddress);
      setTotalItems(res.data.totalPages);
      console.log(999, cityFilter, districtFilter, textSearch);
    });
  };

  const handleChangePage = (page) => {
    setPageNumber(page);
  };

  const ChangeIsBlock = (communeId) => {
    console.log(111, communeId);
  };

  const OnSearchAddress = (event) => {
    if (event.key === 'Enter') {
      loadData();
    }
  };

  const OnChangeCityFilter = (val) => {
    setCityFilter(val);
    setDistrictFilter(undefined);
    OnLoadDistrictFilter(val);
  };

  const OnChangeDistrictFilter = (val) => {
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

  useEffect(() => {
    loadData();
  }, [pageNumber, districtFilter, cityFilter]);

  useEffect(() => {
    loadTypeCityOptions();
  }, []);

  return (
    <>
      <div className='w-[100%]'>
        <div className='mb-[16px] flex items-center'>
          <AutoComplete
            style={{ width: 300 }}
            placeholder='Tìm kiếm tên địa điểm (enter để tìm kiếm)'
            className='mt-4 mb-4'
            onSearch={(val) => setTextSearch(val)}
            allowClear={{ clearIcon: <CloseSquareFilled /> }}
            onKeyDown={OnSearchAddress} />
          <Select
            className='w-80'
            showSearch
            placeholder='Tỉnh/Thành phố'
            optionFilterProp='children'
            allowClear
            onChange={OnChangeCityFilter}
            filterOption={OnFilterOption}
            options={typeCityOptions}
          />
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
        </div>

        <Table columns={columns} dataSource={data} pagination={false} />
        <div className='py-[16px] flex flex-row-reverse pr-[24px]'>
          <Pagination current={pageNumber} pageSize={pageSize} onChange={handleChangePage} total={totalItems}
                      showSizeChanger={false} />
        </div>
      </div>
    </>
  );
}

export default ListAddressManager;