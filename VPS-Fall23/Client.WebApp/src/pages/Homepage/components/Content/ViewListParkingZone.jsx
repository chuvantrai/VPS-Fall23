import { Table, Space } from 'antd';
import { useEffect, useState } from 'react';
import useParkingZoneService from '@/services/parkingZoneService.js'
import SearchBar from '../../../../components/searchbar/SearchBar';

const columns = [
  {
    title: 'Name',
    dataIndex: 'name',
  },
  {
    title: 'Owner',
    dataIndex: 'owner',
  },
  {
    title: 'Created',
    dataIndex: 'created',
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

const onChange = (pagination, filters, sorter, extra) => {
  console.log('params', pagination, filters, sorter, extra);
};

function ViewListParkingZone() {
  const parkingZoneService = useParkingZoneService();

  const [data, setData] = useState([{ id: '', name: '', owner: '', created: Date }]);
  let dataShow = [{ key: '', name: '', owner: '', created: Date }];
  let searchData = [{ label: '', value: '' }]

  useEffect(() => {
    getData();
  }, []);

  const getData = async () => {
    await parkingZoneService.getAllParkingZone().then((res) => {
      setData(res.data);
    })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
  };

  return (
    <div className="w-full px-4">
      {data !== undefined &&
        data.map((val) => {
          const item = { key: val.id, name: val.name, owner: val.owner, created: val.created };
          const searchItem = { label: val.name, value: val.id }
          dataShow.push(item);
          searchData.push(searchItem);
        })}
      {dataShow.shift() && console.log(dataShow)}
      {searchData.shift() && console.log(searchData)}
      <SearchBar data={searchData}></SearchBar>
      {dataShow.length != 1 && dataShow[0].key !== '' && (
        <Table columns={columns} dataSource={dataShow} onChange={onChange} />
      )}
    </div>
  );
}

export default ViewListParkingZone;
