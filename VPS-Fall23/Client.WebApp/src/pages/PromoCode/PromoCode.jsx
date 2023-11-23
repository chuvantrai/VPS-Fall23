/* eslint-disable no-unused-vars */
/* eslint-disable react-hooks/exhaustive-deps */
import { Button, Table, Pagination, notification } from 'antd';
import { PlusCircleOutlined, EditOutlined } from '@ant-design/icons'
import { useEffect, useState } from 'react';

import usePromoCodeServices from '@/services/promoCodeServices';
import { getAccountJwtModel } from '@/helpers';
import moment from 'moment';
import ModalAddCode from './components/ModalAddCode';
import ModalDetailCode from './components/ModalDetailCode';

function PromoCode() {
  const promoCodeServices = usePromoCodeServices();
  const account = getAccountJwtModel();

  const [data, setData] = useState([])
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [openAddCode, setOpenAddCode] = useState(false)
  const [confirmLoading, setConfirmLoading] = useState(false);
  const [confirmDetailLoading, setConfirmDetailLoading] = useState(false);
  const [openDetailCode, setOpenDetailCode] = useState(false)
  const [promoCodeId, setPromoCodeId] = useState('')

  const columns = [
    {
      title: 'Code',
      dataIndex: 'code',
      key: 'code'
    },
    {
      title: 'Giảm giá (%)',
      key: 'discount',
      dataIndex: 'discount'
    },
    {
      title: 'Số lần sử dụng',
      key: 'numberOfUses',
      dataIndex: 'numberOfUses'
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
      title: '',
      key: 'action',
      render: (_, record) => (
        <a
          onClick={(e) => {
            e.preventDefault();
            setOpenDetailCode(true)
            setPromoCodeId(record.key)
          }}
        >
          <EditOutlined />
        </a>
      )
    },
  ];

  const getData = () => {
    promoCodeServices.getListPromoCode(account.UserId, pageNumber)
      .then(res => {
        const obj = res.data.data.map(val => ({
          key: val.id,
          code: val.code,
          fromDate: moment(val.fromDate).format('DD-MM-yyyy'),
          toDate: moment(val.toDate).format('DD-MM-yyyy'),
          discount: val.discount,
          numberOfUses: val.numberOfUses
        }))
        setData(obj)
        setTotalItems(res.data.totalCount);
      })
  }

  useEffect(() => {
    getData()
  }, [pageNumber])

  const handleChangePage = (page) => {
    setPageNumber(page);
  };

  const createNewCode = (values) => {
    let input = {
      ownerId: values.ownerId,
      code: values.code,
      discount: values.discount,
      numberOfUses: values.numberOfUses,
      parkingZoneIds: values.parkingZoneIds,
      fromDate: values.selectedDate[0],
      toDate: values.selectedDate[1]
    }
    setConfirmLoading(true)
    promoCodeServices.createNewPromoCode(input)
      .then(res => {
        notification.success({
          message: res.data,
        });
        getData()
        setConfirmLoading(false)
        setOpenAddCode(false);
      })
  }

  const updatePromoCode = (values) => {
    setConfirmDetailLoading(true)
    let input = {
      promoCodeId: values.promoCodeId,
      promoCode: values.code,
      discount: values.discount,
      numberOfUses: values.numberOfUses,
      parkingZoneIds: values.parkingZoneIds,
      fromDate: values.selectedDate[0],
      toDate: values.selectedDate[1]
    }
    promoCodeServices.updatePromoCode(input)
      .then(res => {
        notification.success({
          message: res.data,
        });
        getData()
        setConfirmDetailLoading(false)
        setOpenDetailCode(false);
      })
  }

  return (
    <div className="w-[100%]">
      <div className='flex flex-row-reverse mb-[16px]'>
        <Button type="primary" icon={<PlusCircleOutlined />} onClick={() => { setOpenAddCode(true) }}>
          Thêm Mã Mới
        </Button>
      </div>

      <Table columns={columns} dataSource={data} pagination={false} />
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
    </div >
  );
}

export default PromoCode;