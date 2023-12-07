import { Button, DatePicker, Divider, Form, Input, Result, Slider, Space, Statistic, Tag, Typography } from 'antd';
import { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import useParkingTransactionService from '@/services/parkingTransactionSerivce';
import dayjs from 'dayjs';
import useParkingZoneAbsentServices from '@/services/parkingZoneAbsentServices';
import usePromoService from '@/services/promoService';
import BookingDescription from './BookingDescription';
const range = (start, end) => {
  const result = [];
  for (let i = start; i < end; i++) {
    result.push(i);
  }
  return result;
};
const defaultPaymentResult = {
  isPaymentRequested: false, isShow: false, paymentTransaction: {}, parkingTransaction: {},
};


// eslint-disable-next-line react/prop-types
const BookingForm = ({ parkingZone }) => {

  const [form] = Form.useForm();
  const [paymentResult, setPaymentResult] = useState(defaultPaymentResult);
  const [isSubmitBtnDisabled, disableSubmitBtn] = useState(false);
  const [absents, setAbsents] = useState([]);
  const [bookingTime, setBookingTime] = useState([null, null]);
  const [promoCode, setPromoCode] = useState({ info: null, message: null });
  const parkingTransactionService = useParkingTransactionService();
  const parkingZoneAbsentService = useParkingZoneAbsentServices();
  const promoService = usePromoService();
  const searchPromo = (promoCode) => {
    promoService
      .getByCode(promoCode, parkingZone.id)
      .then(res => {
        setPromoCode({
          info: res.data
        })
      })
      .catch(error => setPromoCode({ info: null, message: error.message }))
  }
  useEffect(() => {
    parkingZoneAbsentService
      .getAbsents(parkingZone.id)
      .then(res => setAbsents(res.data))

    return () => {
      form.resetFields();
      setPaymentResult(defaultPaymentResult);
      setBookingTime([null, null]);
      setPromoCode({ info: null, message: null })
      disableSubmitBtn(false);
    }
  }, [])
  const getDisabledDate = (date) => {
    var absentToBool = absents.map((a) => {
      return (!a.from || dayjs(a.from)) <= date && (!a.to || date <= dayjs(a.to))
    })
    return date && date < dayjs().startOf('day') || absentToBool.includes(true);
  }
  const getDisableTime = (date, partial) => {
    return {
      disabledHours: () => {
        let startTime = Number(parkingZone?.workFrom.split(':')[0]) ?? 6
        let endTime = Number(parkingZone?.workTo.split(':')[0]) ?? 23
        var hour = dayjs().hour()
        if (startTime <= hour && (date && date <= dayjs().endOf('day')) && partial == 'start') startTime = hour
        return [...range(0, startTime), ...range(endTime, 24)]
      },
    }
  }
  const onSubmitClick = () => {

    form.validateFields().then(form => {

      let parkingTransaction = {
        ...form,
        // eslint-disable-next-line react/prop-types
        parkingZoneId: parkingZone.id,
        checkinAt: form.ioTime[0],
        checkoutAt: form.ioTime[1],
        licensePlate: form.licensePlate.toUpperCase(),
      };
      console.log(parkingTransaction);
      book(parkingTransaction);
    });
  };

  let connection = new HubConnectionBuilder()
    .withUrl(import.meta.env.VITE_API_GATEWAY + '/payment')
    .withAutomaticReconnect()
    .build();

  const requestPayment = (parkingTransaction) => {
    parkingTransactionService
      .getPaymentUrl(parkingTransaction.id)
      .then(res => {
        var url = new URL(res.data);
        var txnRef = url.searchParams.get('vnp_TxnRef');
        connection.start().then(() => {
          connection.invoke('RegisterPayment', connection.connectionId, txnRef);
        });
        connection.on('ReceivePaidStatus', (paymentTransaction) => {
          setPaymentResult({
            ...paymentResult,
            isPaymentRequested: true,
            isShow: true,
            paymentTransaction: JSON.parse(paymentTransaction),
            parkingTransaction: parkingTransaction,
          });
          disableSubmitBtn(false);
        });
        window.open(res.data, '_blank');
      });
  };
  const book = (parkingTransaction) => {
    if (paymentResult.isPaymentRequested) {
      requestPayment(paymentResult.parkingTransaction);
      return;
    }
    parkingTransactionService
      .bookingSlot(parkingTransaction)
      .then((res) => {
        disableSubmitBtn(true);
        setPaymentResult({ ...paymentResult, parkingTransaction: res.data });
        requestPayment(res.data);
      });
  };

  const getPaymentResultProps = () => {
    console.log(paymentResult);
    const success = paymentResult?.paymentTransaction.ResponseCode == 0 && paymentResult?.paymentTransaction.TransactionStatus == 0;
    return {
      status: (success) ? 'success' : 'error',
      title: `Thanh toán đặt lịch gửi xe ${success ? 'thành công' : 'thất bại'}`,
      subTitle: (<>
        <p>Số đơn hàng: {paymentResult?.paymentTransaction.TxnRef}</p>
        <p>Mã giao dịch: {paymentResult?.paymentTransaction.TransactionNo}</p>
        <p>Nội dung giao dịch: {paymentResult?.paymentTransaction.OrderInfo}</p>
        <Button onClick={() => setPaymentResult(defaultPaymentResult)}>OK</Button>
      </>),
    };
  };
  return (<>
    <Form
      labelCol={{ span: 6 }}
      wrapperCol={{ span: 14 }}
      form={form}
      hidden={paymentResult.isShow}
    >
      <Form.Item
        label='Email của bạn'
        name='email'
        rules={[
          { required: true, message: 'Vui lòng nhập email của bạn' },
          {
            type: 'email',
            message: 'Email không hợp lệ',
          },
        ]}
      >
        <Input placeholder='Email của bạn'></Input>
      </Form.Item>
      <Form.Item
        label='Số điện thoại'
        name='phone'
        rules={
          [
            {
              required: true, message: 'Vui lòng nhập số điện thoại của bạn',
            },
            {
              pattern: /(0[3|5|7|8|9])+([0-9]{8})\b/g,
              message: 'Số điện thoại không hợp lệ',
            },
          ]
        }
      >
        <Input placeholder='Số điện thoại của bạn'></Input>
      </Form.Item>
      <Form.Item
        label='Biển số xe'
        name='licensePlate'
        rules={[
          { required: true, message: "Vui lòng nhập biển số xe cần gửi" },
          { pattern: /^[a-zA-Z0-9\-\.]/g, message: "Biển số xe không hợp lệ" }
        ]}
      >
        <Input placeholder='Nhập biển số xe cần gửi' />
      </Form.Item>
      <Form.Item
        label='Thời gian vào/ra'
        name='ioTime'
        rules={[
          { required: true, message: 'Vui lòng chọn thời gian vào/ra' },

        ]}
      >
        <DatePicker.RangePicker
          placement='bottomLeft'
          showTime={{ format: 'HH', hourStep: 1, hideDisabledOptions: true }}
          format="YYYY-MM-DD HH"
          placeholder={["Thời gian gửi xe vào", "Thời gian lấy xe ra"]}
          disabledDate={getDisabledDate}
          disabledTime={getDisableTime}
          onChange={(values) => {
            if (values == null) {
              setBookingTime([null, null])
              return;
            }
            setBookingTime([values[0], values[1]])
          }}
        />
      </Form.Item>
      <Form.Item
        label='Mã giảm giá'
        name='promoCode'

      >
        <Input.Search
          type='search'
          name='promoCode'
          placeholder='Nhập mã giảm giá (nếu có)'
          onSearch={(value) => searchPromo(value)}
          onChange={() => setPromoCode({ info: null, message: null })}
        />
      </Form.Item>
      <Divider></Divider>
      <BookingDescription
        pricePerHour={parkingZone.pricePerHour}
        ioTime={bookingTime}
        discount={promoCode?.info?.promoCodeInformation?.discount ?? 0}
      >

      </BookingDescription>
      <Form.Item className={('flex justify-center m-0')}>
        <Button className={('bg-[#1890FF]')}
          type='primary'
          onClick={onSubmitClick}
          disabled={isSubmitBtnDisabled}
        >
          Đặt chỗ
        </Button>
      </Form.Item>
    </Form>
    {paymentResult.isShow === true && <Result
      {...getPaymentResultProps()}
    />}

  </ >);
};

export default BookingForm;
