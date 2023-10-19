import { Table } from 'antd';
import { useEffect, useState } from 'react';
import service from '@/services/parkingZoneService.js'

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
];

const onChange = (pagination, filters, sorter, extra) => {
  console.log('params', pagination, filters, sorter, extra);
};

function ViewListParkingZone() {
  const [data, setData] = useState([{ name: '', owner: '', created: Date }]);
  let dataShow = [{ key: '', name: '', owner: '', created: Date }];

  useEffect(() => {
    getData();
  }, []);

  const getData = async () => {
    await service.getAllParkingZone().then((res) => {
      setData(res.data);
    })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
    // await axios
    //   .get('api/ParkingZone/GetAll')
    //   .then((res) => {
    //     setData(res.data);
    //   })
    //   .catch((error) => {
    //     console.error('Error fetching data:', error);
    //   });
  };

  return (
    <div className="w-full px-4">
      {data !== undefined && //console.log(data)
        data.map((val, index) => {
          const item = { key: index, name: val.name, owner: val.owner, created: val.created };
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
