/* eslint-disable no-unused-vars */
/* eslint-disable react-hooks/exhaustive-deps */
import { Button, Table, Pagination, Divider, Popconfirm, notification } from 'antd';
import { PlusCircleOutlined, EditOutlined, DeleteOutlined, QuestionCircleOutlined } from '@ant-design/icons';
import { useEffect, useState } from 'react';

import usePromoCodeServices from '@/services/promoCodeServices';
import { getAccountJwtModel } from '@/helpers';
import moment from 'moment';
import ModalAddCode from './components/ModalAddCode';
import ModalDetailCode from './components/ModalDetailCode';

function PromoCode() {
  const promoCodeServices = usePromoCodeServices();
  const account = getAccountJwtModel();

  const [data, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [openAddCode, setOpenAddCode] = useState(false);
  const [confirmLoading, setConfirmLoading] = useState(false);
  const [confirmDetailLoading, setConfirmDetailLoading] = useState(false);
  const [openDetailCode, setOpenDetailCode] = useState(false);
  const [promoCodeId, setPromoCodeId] = useState('');

  const columns = [
    {
      title: 'Mã giảm (%)',
      key: 'discount',
      dataIndex: 'discount',
    },
    {
      title: 'Ngày bắt đầu',
      dataIndex: 'fromDate',
      key: 'fromDate',
    },
    {
      title: 'Ngày kết thúc',
      dataIndex: 'toDate',
      key: 'toDate',
    },
    {
      title: 'Danh sách bãi đỗ xe được áp dụng',
      dataIndex: 'parkingZones',
      key: 'parkingZones',
      render: (_, record) => {
        let arr = record.parkingZones;
        return arr?.map((item, index) => {
          if (arr.length === 1) {
            return item.name;
          } else if (arr.length > 1 && arr.length <= 3) {
            return index < arr.length - 1 ? item.name + ', ' : item.name;
          } else if (arr.length > 3) {
            return index < 2 ? item.name + ', ' : index === 2 ? item.name + ', ...' : '';
          }
        })
      }
    },
    {
      title: '',
      key: 'action',
      render: (_, record) => (
        <>
          <Button
            onClick={(e) => {
              e.preventDefault();
              setOpenDetailCode(true);
              setPromoCodeId(record.key);
            }}
            icon={<EditOutlined />}
          ></Button>
          <Divider type="vertical" />
          <Popconfirm
            title="Xóa ưu đãi"
            description="Bạn có chắc muốn xóa toàn bộ ưu đãi này?"
            icon={
              <QuestionCircleOutlined
                style={{
                  color: 'red',
                }}
              />
            }
            onConfirm={() => {
              handleDeletePromoCode(record.key);
            }}
          >
            <Button danger icon={<DeleteOutlined />}></Button>
          </Popconfirm>
        </>
      ),
    },
  ];

  const getData = () => {
    promoCodeServices.getListPromoCode(account.UserId, pageNumber).then((res) => {
      const obj = res.data.data.map((val) => ({
        key: val.id,
        fromDate: moment(val.fromDate).format('DD-MM-yyyy'),
        toDate: moment(val.toDate).format('DD-MM-yyyy'),
        discount: val.discount,
        promoCodes: val.promoCodes,
        parkingZones: val.parkingZones
      }));
      setData(obj);
      setTotalItems(res.data.totalCount);
    });
  };

  useEffect(() => {
    getData();
  }, [pageNumber]);

  const handleChangePage = (page) => {
    setPageNumber(page);
  };

  const createNewCode = (values) => {
    let input = {
      ownerId: values.ownerId,
      discount: values.discount,
      parkingZoneIds: values.parkingZoneIds,
      fromDate: values.selectedDate[0],
      toDate: values.selectedDate[1],
    };
    setConfirmLoading(true);
    promoCodeServices
      .createNewPromoCode(input)
      .then((res) => {
        notification.success({
          message: res.data,
        });
        getData();
        setConfirmLoading(false);
        setOpenAddCode(false);
      })
      .catch((err) => {
        setConfirmLoading(false);
        setOpenAddCode(false);
      });
  };

  const updatePromoCode = (values) => {
    setConfirmDetailLoading(true);
    let input = {
      promoCodeId: values.promoCodeId,
      discount: values.discount,
      parkingZoneIds: values.parkingZoneIds,
      fromDate: values.selectedDate[0],
      toDate: values.selectedDate[1],
    };
    promoCodeServices.updatePromoCode(input).then((res) => {
      notification.success({
        message: res.data,
      });
      getData();
      setConfirmDetailLoading(false);
      setOpenDetailCode(false);
    })
      .catch(err => {
        setConfirmDetailLoading(false);
        setOpenDetailCode(false);
      });
  };

  const handleDeletePromoCode = (promoCodeId) => {
    promoCodeServices.deletePromoCode(promoCodeId).then((res) => {
      notification.success({
        message: res.data,
      });
      getData();
    });
  };

  return (
    <div className="w-[100%]">
      <div className="flex flex-row-reverse mb-[16px]">
        <Button
          type="primary"
          icon={<PlusCircleOutlined />}
          onClick={() => {
            setOpenAddCode(true);
          }}
        >
          Thêm Mã Mới
        </Button>
      </div>

      <Table
        columns={columns}
        dataSource={data}
        pagination={false}
        expandable={{
          expandedRowRender: (record) => (
            <>
              <p className="font-medium">Danh sách Mã giảm giá:</p>
              {record.promoCodes?.map((item, index) => {
                if (record.promoCodes?.length === index + 1) {
                  return `${item.code}`
                } else {
                  return `${item.code} - `
                }
              })}
            </>
          ),
        }}
      />
      <div className="py-[16px] flex flex-row-reverse pr-[24px]">
        <Pagination current={pageNumber} onChange={handleChangePage} total={totalItems} showSizeChanger={false} />
      </div>

      <ModalAddCode
        open={openAddCode}
        confirmLoading={confirmLoading}
        onCreate={createNewCode}
        onCancel={() => {
          setOpenAddCode(false);
        }}
      />

      <ModalDetailCode
        open={openDetailCode}
        confirmLoading={confirmDetailLoading}
        promoCodeId={promoCodeId}
        onUpdate={updatePromoCode}
        onCancel={() => setOpenDetailCode(false)}
      />
    </div>
  );
}

export default PromoCode;
