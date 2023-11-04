import { useEffect, useState, Fragment } from 'react';
import { Table, Pagination, AutoComplete } from 'antd';
import useParkingZoneService from '../../../../services/parkingZoneOwnerService';
import moment from 'moment';

const onChange = (pagination, filters, sorter, extra) => {
  console.log('params', pagination, filters, sorter, extra);
};

function ViewListParkingZoneOwner() {
  const columns = [
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
      width: '30%',
    },
    {
      title: 'Phone',
      dataIndex: 'phone',
      key: 'phone',
      width: '20%',
    },
    {
      title: 'Dob',
      dataIndex: 'dob',
      key: 'dob',
      width: '20%',
    },
    {
      title: 'Created',
      dataIndex: 'created',
      key: 'created',
    },
    {
      title: 'Modified',
      dataIndex: 'modified',
      key: 'modified',
    },
  ];

  const ownerSerivce = useParkingZoneService();

  const [data, setData] = useState();
  const [inputValue, setInputValue] = useState('');

  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10;
  const [totalItems, setTotalItems] = useState();

  const formatted = moment().format('YYYY-MM-DD HH:mm:ss');

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
            email: val.email,
            phone: val.phone,
            dob: moment(val.dob).format('DD/MM/YYYY'),
            created: moment(val.createdAt).format('DD/MM/YYYY HH:mm:ss'),
            modified: moment(val.modifiedAt).format('DD/MM/YYYY HH:mm:ss'),
          }));
          setData(obj);
          //   console.log(obj);
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
            email: val.email,
            phone: val.phone,
            dob: moment(val.dob).format('DD/MM/YYYY'),
            created: moment(val.createdAt).format('DD/MM/YYYY HH:mm:ss'),
            modified: moment(val.modifiedAt).format('DD/MM/YYYY HH:mm:ss'),
          }));
          setData(obj);
          //   console.log(obj);
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
    </Fragment>
  );
}

export default ViewListParkingZoneOwner;
