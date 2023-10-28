import { useState } from 'react';
import { Button, Table, Space, Tag, notification } from 'antd';

import useAttendantService from '@/services/attendantServices';
import ModalAdd from './components/ModalAdd';

const columns = [
  {
    title: 'Name',
    dataIndex: 'name',
    key: 'name',
    render: (text) => <a>{text}</a>,
  },
  {
    title: 'Age',
    dataIndex: 'age',
    key: 'age',
  },
  {
    title: 'Address',
    dataIndex: 'address',
    key: 'address',
  },
  {
    title: 'Tags',
    key: 'tags',
    dataIndex: 'tags',
    render: (_, { tags }) => (
      <>
        {tags.map((tag) => {
          let color = tag.length > 5 ? 'geekblue' : 'green';
          if (tag === 'loser') {
            color = 'volcano';
          }
          return (
            <Tag color={color} key={tag}>
              {tag.toUpperCase()}
            </Tag>
          );
        })}
      </>
    ),
  },
  {
    title: 'Action',
    key: 'action',
    render: (_, record) => (
      <Space size="middle">
        <a>Invite {record.name}</a>
        <a>Delete</a>
      </Space>
    ),
  },
];
const data = [
  {
    key: '1',
    name: 'John Brown',
    age: 32,
    address: 'New York No. 1 Lake Park',
    tags: ['nice', 'developer'],
  },
  {
    key: '2',
    name: 'Jim Green',
    age: 42,
    address: 'London No. 1 Lake Park',
    tags: ['loser'],
  },
  {
    key: '3',
    name: 'Joe Black',
    age: 32,
    address: 'Sydney No. 1 Lake Park',
    tags: ['cool', 'teacher'],
  },
];

function ListAttendant() {
  const service = useAttendantService();

  const [open, setOpen] = useState(false);

  const handleAdd = () => {
    setOpen(true);
  };
  const onCreate = (values) => {
    service.createAccount(values).then((res) => {
      notification.success({
        message: res.data,
      });
      console.log(res);
    });
    setOpen(false);
  };

  return (
    <div className="w-[100%] mt-[20px] mx-[16px]">
      <Button className="mb-[16px] bg-[#1890FF]" onClick={handleAdd} type="primary">
        Thêm tài khoản
      </Button>

      <Table columns={columns} dataSource={data} />

      <ModalAdd
        open={open}
        onCreate={onCreate}
        onCancel={() => {
          setOpen(false);
        }}
      />
    </div>
  );
}

export default ListAttendant;
