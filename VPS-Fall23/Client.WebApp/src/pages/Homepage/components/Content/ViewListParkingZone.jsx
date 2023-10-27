import { Fragment, useEffect, useState } from 'react';
import { Space, Table, Pagination, AutoComplete } from 'antd';
import { useDebounce } from 'use-debounce';

import useParkingZoneService from '@/services/parkingZoneService.js'
// import ColumnSearchProps from '../../../../components/searchbar/ColumnSearchProps'

const onChange = (pagination, filters, sorter, extra) => {
  console.log('params', pagination, filters, sorter, extra);
};

function ViewListParkingZone() {
  const parkingZoneService = useParkingZoneService();

  const [data, setData] = useState([{ id: '', name: '', owner: '', created: Date }]);
  let dataShow = [{ key: '', name: '', owner: '', created: Date }];
  const [inputValue, setInputValue] = useState('');

  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10;
  const [totalItems, setTotalItems] = useState();

  // eslint-disable-next-line no-unused-vars
  const [debouncedValue] = useDebounce(inputValue, 300);


  const handleSearch = (e) => {
    searchDataByName(e);
    setInputValue(e);
  };

  const columns = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      width: '30%',
    },
    {
      title: 'Owner',
      dataIndex: 'owner',
      key: 'owner',
      width: '20%',
    },
    {
      title: 'Created',
      dataIndex: 'created',
      key: 'created',
    },
    {
      title: '',
      key: 'action',
      render: () => (
        <Space size="middle">
          <a>Detail</a>
        </Space>
      ),
    },
  ];

  useEffect(() => {
    getData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const getData = async (currentPage) => {
    await parkingZoneService.getAllParkingZone({ pageNumber: currentPage, pageSize: pageSize }).then((res) => {
      setData(res?.data.data);
      setTotalItems(res?.data.totalCount);
    })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
  };

  const searchDataByName = async (e) => {
    if (e !== "") {
      await parkingZoneService.getParkingZoneByName({ pageNumber: currentPage, pageSize: pageSize, name: e }).then((res) => {
        setData(res?.data.data);
        setTotalItems(res?.data.totalCount);
      })
        .catch((error) => {
          console.error('Error fetching data:', error);
        });
    }
  }

  const handleChangePage = (page) => {
    setCurrentPage(page);
    getData(page);
  };

  return (
    <div className="w-full px-4">
      <AutoComplete
        style={{ width: 200 }}
        onSearch={handleSearch}
        placeholder="input here"
        className='mt-4 mb-4'
      />
      {data !== undefined &&
        data.map((val) => {
          const item = { key: val.id, name: val.name, owner: val.owner, created: val.created };
          dataShow.push(item);
        })}
      {dataShow.shift() && dataShow.length != 1 && dataShow[0].key !== '' && (
        <Fragment>
          <Table columns={columns} dataSource={dataShow} onChange={onChange} pagination={false}
          />
          <div className="py-[16px] flex flex-row-reverse pr-[24px]">
            <Pagination current={currentPage} onChange={handleChangePage} total={totalItems} showSizeChanger={false} />
          </div>
        </Fragment>
      )}
    </div>
  );
}

export default ViewListParkingZone;
