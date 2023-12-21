/* eslint-disable no-unused-vars */
/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useRef, useState } from 'react';
import { Button, Table, notification, Pagination, Input, Row, Col, Tooltip, Popconfirm } from 'antd';
import { LockFilled, UnlockFilled, UserAddOutlined, QuestionCircleOutlined } from '@ant-design/icons';
import { useDebounce } from 'use-debounce';

import useAttendantService from '@/services/attendantServices';
import AccountServices from '@/services/accountServices';
import { getAccountJwtModel } from '@/helpers';
import ModalAdd from './components/ModalAdd';
import ModalBlockReason from './components/ModalBlockReason';

function ListAttendant() {
  const service = useAttendantService();
  const account = getAccountJwtModel();
  const accountService = AccountServices();

  const [open, setOpen] = useState(false);
  const [blockOpen, setBlockOpen] = useState(false);
  const [data, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [searchText, setSearchText] = useState('')
  const [blockAccountId, setBlockAccountId] = useState('');
  const [confirmLoading, setConfirmLoading] = useState(false);

  const [debounceValue] = useDebounce(searchText, 500);

  const columns = [
    {
      title: 'Username',
      dataIndex: 'username',
      key: 'username'
    },
    {
      title: 'Họ và Tên',
      dataIndex: 'fullName',
      key: 'fullName',
    },
    {
      title: 'Địa chỉ',
      dataIndex: 'address',
      key: 'address',
    },
    {
      title: 'Số điện thoại',
      key: 'phoneNumber',
      dataIndex: 'phoneNumber'
    },
    {
      title: 'Bãi đỗ xe làm việc',
      key: 'parkingZone',
      dataIndex: 'parkingZone'
    },
    {
      title: '',
      key: 'action',
      render: (_, record) => (
        <>
          {record.isBlock === false ?
            <Tooltip title="Khóa tài khoản">
              <Popconfirm
                title="Khóa tài khoản"
                description="Bạn chắc chắn muốn khóa tài khoản này?"
                icon={
                  <QuestionCircleOutlined
                    style={{
                      color: 'red',
                    }}
                  />
                }
                onConfirm={() => handleOpenBlockModal(record)}
              >
                <Button className='bg-red-600 flex items-center'>
                  <LockFilled className='text-white' />
                </Button>
              </Popconfirm>
            </Tooltip>
            :
            <Tooltip title="Mở khóa tài khoản">
              <Popconfirm
                title="Mở khóa tài khoản"
                description="Bạn chắc chắn muốn mở khóa tài khoản này?"
                icon={
                  <QuestionCircleOutlined
                    style={{
                      color: 'green',
                    }}
                  />
                }
                onConfirm={() => handleUnblockAccount(record)}
              >
                <Button className='bg-green-600 flex items-center'>
                  <UnlockFilled className='text-white' />
                </Button>
              </Popconfirm>
            </Tooltip>
          }
        </>
      ),
    },
  ];

  const handleOpenBlockModal = (record) => {
    setBlockOpen(true)
    setBlockAccountId(record.id)
  }
  const onBlock = (values) => {
    accountService.blockAccount(values)
      .then(res => {
        notification.success({
          message: res.data
        })
        getData()
      })
    setBlockOpen(false)
  }

  const handleUnblockAccount = (record) => {
    let input = {
      accountId: record.id,
      isBlock: false
    }
    accountService.blockAccount(input)
      .then(res => {
        notification.success({
          message: res.data
        })
        getData()
      })
  }

  const getData = () => {
    service.getListAttendant(account.UserId, pageNumber)
      .then(res => {
        setData(res.data.data)
        setTotalItems(res.data.totalCount);
      })
  }
  const searchData = () => {
    service.searchAttendantByName(account.UserId, debounceValue, pageNumber)
      .then(res => {
        setData(res.data.data)
        setTotalItems(res.data.totalCount);
      })
  }

  const firstUpdate = useRef(true)
  useEffect(() => {
    if (firstUpdate.current) {
      firstUpdate.current = false;
      getData();
      return;
    }

    if (debounceValue.trim() === '') {
      getData()
    } else {
      searchData()
    }
  }, [debounceValue, pageNumber])

  const handleChangePage = (page) => {
    setPageNumber(page);
  };

  const handleAdd = () => {
    setOpen(true);
  };
  const onCreate = (values) => {
    setConfirmLoading(true)
    service.createAccount(values).then((res) => {
      notification.success({
        message: res.data,
      });
      getData()
      setConfirmLoading(false)
      setOpen(false);
    })
      .catch(err => {
        setConfirmLoading(false)
        setOpen(false);
      });
  };

  return (
    <div className="w-[100%] mt-[20px] mx-[16px]">
      <div className='flex justify-between items-center mb-[16px]'>
        <div>
          <Row>
            <Col span={8} className='flex items-center justify-center'>
              <p>Tìm kiếm <Tooltip title='Nhập tên cần tìm kiếm vào đây'><QuestionCircleOutlined /></Tooltip> :</p>
            </Col>
            <Col span={16}>
              <Input
                placeholder='Nhập tên...'
                allowClear
                onChange={e => {
                  setSearchText(e.target.value)
                }}
              />
            </Col>
          </Row>
        </div>
        <Button className="bg-[#1890FF] flex items-center" onClick={handleAdd} type="primary">
          <UserAddOutlined />Thêm tài khoản
        </Button>
      </div>

      <Table columns={columns} dataSource={data} pagination={false} />
      <div className="py-[16px] flex flex-row-reverse pr-[24px]">
        <Pagination current={pageNumber} onChange={handleChangePage} total={totalItems} showSizeChanger={false} />
      </div>

      <ModalAdd
        open={open}
        confirmLoading={confirmLoading}
        onCreate={onCreate}
        onCancel={() => {
          setOpen(false);
        }}
      />

      <ModalBlockReason
        open={blockOpen}
        accountId={blockAccountId}
        onBlock={onBlock}
        onCancel={() => {
          setBlockOpen(false);
        }}
      />
    </div>
  );
}

export default ListAttendant;
