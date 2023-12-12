import { useEffect, useState, Fragment } from 'react';
import { Table, Pagination, AutoComplete, Popconfirm, Button, notification, Tooltip } from 'antd';
import { QuestionCircleOutlined, LockFilled, UnlockFilled } from '@ant-design/icons'
import useParkingZoneService from '../../../../services/parkingZoneOwnerService';
import moment from 'moment';
import ModalBlockReason from '../../../ListAttendant/components/ModalBlockReason';
import AccountServices from '@/services/accountServices';

const onChange = (pagination, filters, sorter, extra) => {
  console.log('params', pagination, filters, sorter, extra);
};

function ViewListParkingZoneOwner() {
  const columns = [
    {
      title: 'Họ Tên',
      dataIndex: 'fullName',
      key: 'fullName'
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email'
    },
    {
      title: 'Số điện thoại',
      dataIndex: 'phone',
      key: 'phone'
    },
    {
      title: 'Ngày sinh',
      dataIndex: 'dob',
      key: 'dob'
    },
    {
      title: 'Ngày tạo',
      dataIndex: 'created',
      key: 'created'
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
            <Tooltip title='Mở khóa tài khoản'>
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

  const ownerSerivce = useParkingZoneService();
  const accountService = AccountServices();

  const [data, setData] = useState();
  const [inputValue, setInputValue] = useState('');
  const [blockOpen, setBlockOpen] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10;
  const [totalItems, setTotalItems] = useState();
  const [blockAccountId, setBlockAccountId] = useState('');

  const handleOpenBlockModal = (record) => {
    setBlockOpen(true)
    setBlockAccountId(record.key)
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
      accountId: record.key,
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

  const handleSearch = (e) => {
    searchDataByName(e);
    setInputValue(e);
  };

  useEffect(() => {
    getData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const getData = async (currentPage) => {
    if (inputValue === '' || inputValue === undefined || inputValue === null) {
      await ownerSerivce
        .getAllOwner({ pageNumber: currentPage, pageSize: pageSize })
        .then((res) => {
          const obj = res.data.data.map((val) => ({
            key: val.id,
            fullName: val.fullName,
            email: val.email,
            phone: val.phone,
            dob: moment(val.dob).format('DD/MM/YYYY'),
            created: moment(val.createdAt).format('DD/MM/YYYY'),
            isBlock: val.isBlock
          }));
          setData(obj);
          setTotalItems(res?.data.totalCount);
        })
        .catch((error) => {
          console.error('Error fetching data:', error);
        });
    }
  };

  const searchDataByName = async (e) => {
    if (e !== '') {
      await ownerSerivce
        .getOwnerByEmail({ pageNumber: currentPage, pageSize: pageSize, email: e })
        .then((res) => {
          const obj = res.data.data.map((val) => ({
            key: val.id,
            fullName: val.fullName,
            email: val.email,
            phone: val.phone,
            dob: moment(val.dob).format('DD/MM/YYYY'),
            created: moment(val.createdAt).format('DD/MM/YYYY HH:mm:ss'),
            modified: moment(val.modifiedAt).format('DD/MM/YYYY HH:mm:ss'),
            isBlock: val.isBlock
          }));
          setData(obj);
          setTotalItems(res?.data.totalCount);
        })
        .catch((error) => {
          console.error('Error fetching data:', error);
        });
    }
    else {
      await ownerSerivce
        .getAllOwner({ pageNumber: currentPage, pageSize: pageSize })
        .then((res) => {
          const obj = res.data.data.map((val) => ({
            key: val.id,
            fullName: val.fullName,
            email: val.email,
            phone: val.phone,
            dob: moment(val.dob).format('DD/MM/YYYY'),
            created: moment(val.createdAt).format('DD/MM/YYYY'),
            isBlock: val.isBlock
          }));
          setData(obj);
          setTotalItems(res?.data.totalCount);
        })
        .catch((error) => {
          console.error('Error fetching data:', error);
        });
    }
  };

  const handleChangePage = (page) => {
    setCurrentPage(page);
    getData(page);
  };

  return (
    <Fragment>
      <div className="w-full px-4">
        <AutoComplete style={{ width: 200 }} onSearch={handleSearch} placeholder="Tìm kiếm" className="mt-4 mb-4" />
        {data != undefined && (
          <Fragment>
            <Table columns={columns} dataSource={data} onChange={onChange} pagination={false} />
            <div className="py-[16px] flex flex-row-reverse pr-[24px]">
              <Pagination
                current={currentPage}
                onChange={handleChangePage}
                total={totalItems}
                showSizeChanger={false}
              />
            </div>
          </Fragment>
        )}
      </div>

      <ModalBlockReason
        open={blockOpen}
        accountId={blockAccountId}
        onBlock={onBlock}
        onCancel={() => {
          setBlockOpen(false);
        }}
      />
    </Fragment>
  );
}

export default ViewListParkingZoneOwner;
