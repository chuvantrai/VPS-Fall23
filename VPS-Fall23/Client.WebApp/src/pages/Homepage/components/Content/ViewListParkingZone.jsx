import { useEffect, useState } from 'react';
import { Space, Table } from 'antd';

import useParkingZoneService from '@/services/parkingZoneService.js'
import ColumnSearchProps from '../../../../components/searchbar/ColumnSearchProps'

const onChange = (pagination, filters, sorter, extra) => {
  console.log('params', pagination, filters, sorter, extra);
};

function ViewListParkingZone() {
  const parkingZoneService = useParkingZoneService();
  const getColumnSearchProps = ColumnSearchProps();

  const [data, setData] = useState([{ id: '', name: '', owner: '', created: Date }]);
  let dataShow = [{ key: '', name: '', owner: '', created: Date }];

  const columns = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      width: '30%',
      ...getColumnSearchProps('name'),
    },
    {
      title: 'Owner',
      dataIndex: 'owner',
      key: 'owner',
      width: '20%',
      ...getColumnSearchProps('owner'),
    },
    {
      title: 'Created',
      dataIndex: 'created',
      key: 'created',
      ...getColumnSearchProps('created'),
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
          dataShow.push(item);
        })}
      {dataShow.shift() && console.log(dataShow)}
      {dataShow.length != 1 && dataShow[0].key !== '' && (
        <Table columns={columns} dataSource={dataShow} onChange={onChange} />
      )}
    </div>
  );
}

export default ViewListParkingZone;
