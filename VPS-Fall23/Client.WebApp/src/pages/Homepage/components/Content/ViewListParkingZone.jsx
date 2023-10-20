import { Table } from 'antd';
import { FormOutlined } from '@ant-design/icons';
import { useEffect, useState } from 'react';

import useParkingZoneService from '@/services/parkingZoneService.js'

const columns = [
  {
    title: 'Tên',
    dataIndex: 'name',
  },
  {
    title: 'Chủ sở hữu',
    dataIndex: 'owner',
  },
  {
    title: 'Ngày tạo',
    dataIndex: 'created',
  },
  {
    title: 'Action',
    key: 'action',
    render: () => (
      <a><FormOutlined /></a>
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
      {console.log(data)}
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
