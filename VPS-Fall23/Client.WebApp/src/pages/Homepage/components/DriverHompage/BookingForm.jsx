import { Button, Form, Input, Result, Slider, Space } from 'antd';
import { useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import useParkingTransactionService from '../../../../services/parkingTransactionSerivce';

const TIME_STEP_IN_HOUR = 1;
const defaultPaymentResult = {
  isPaymentRequested: false, isShow: false, paymentTransaction: {}, parkingTransaction: {},
};
// eslint-disable-next-line react/prop-types
const BookingForm = ({ parkingZone }) => {

  const [form] = Form.useForm();
  const [paymentResult, setPaymentResult] = useState(defaultPaymentResult);
  const parkingTransactionService = useParkingTransactionService();
  const onSubmitClick = () => {
    form.validateFields().then(form => {

      let parkingTransaction = {
        ...form,
        // eslint-disable-next-line react/prop-types
        parkingZoneId: parkingZone.id,
        checkinAt: `${bookingTime[0]}:00:00`,
        checkoutAt: `${bookingTime[1]}:00:00`,
        licensePlate: `${form.licensePlatePre.trim()}-${form.licensePlateMid.trim()}${form.licensePlateEnd.trim()}`.toUpperCase(),
      };
      book(parkingTransaction);
    });
  };
  // const onClose = () => {
  //   form.resetFields();
  //   setPaymentResult({ ...paymentResult, isShow: false });
  //   store.dispatch(setShowBookingForm({ isShowBookingForm: false }));
  // };
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
            paymentTransaction: paymentTransaction,
            parkingTransaction: parkingTransaction,
          });
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
        setPaymentResult({ ...paymentResult, parkingTransaction: res.data });
        requestPayment(res.data);
      });
  };
  const [bookingTime, setBookingTime] = useState();
  const getPaymentResultProps = () => {
    const success = paymentResult?.paymentTransaction.responseCode == '00' && paymentResult?.paymentTransaction.transactionStatus == '00';
    return {
      status: (success) ? 'success' : 'error',
      title: `Thanh toán đặt lịch gửi xe ${success ? 'thành công' : 'thất bại'}`,
      subTitle: (<>
        <p>Số đơn hàng: {paymentResult?.paymentTransaction.txnRef}</p>
        <p>Mã giao dịch: {paymentResult?.paymentTransaction.transactionNo}</p>
        <Button onClick={() => setPaymentResult(defaultPaymentResult)}>OK</Button>
      </>),
    };
  };
  return (<    >
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
        <Input></Input>
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
        <Input></Input>
      </Form.Item>
      <Form.Item
        label='Biển số xe'
        rules={[{ required: true }]}
      >
        <Space.Compact>
          <Form.Item
            name='licensePlatePre'
            noStyle
            rules={[
              { required: true, message: 'Vui lòng nhập biển số xe' },
              { pattern: /^[0-9]{2}/gm, message: '' },
            ]}
          >
            <Input style={{ width: '30%' }} required addonAfter='-' />
          </Form.Item>
          <Form.Item
            name='licensePlateMid'
            noStyle
            rules={[
              { required: true, message: 'Vui lòng nhập biển số xe' },
              { pattern: /^[0-9A-Za-z]/gm, message: '' },
            ]}
          >
            <Input style={{ width: '30%' }} required />
          </Form.Item>
          <Form.Item
            name='licensePlateEnd'
            noStyle
            rules={[
              { required: true, message: 'Vui lòng nhập biển số xe' },
            ]}
          >
            <Input style={{ width: '40%' }} type='number' required />
          </Form.Item>
        </Space.Compact>
      </Form.Item>
      <Form.Item
        label='Thời gian vào/ra'
        name='checkinTime'
        rules={[{ required: true, message: 'Vui lòng chọn thời gian vào/ra' }]}
      >
        <Slider
          range={{ draggableTrack: true }}
          min={Number(parkingZone?.workFrom.split(':')[0]) ?? 6}
          max={Number(parkingZone?.workTo.split(':')[0]) ?? 23}
          step={TIME_STEP_IN_HOUR}
          tooltip={{
            autoAdjustOverflow: true,
            formatter: (val) => `${val}h`,
            placement: 'bottom',
          }}
          value={[bookingTime]}
          onChange={setBookingTime}
        >

        </Slider>

      </Form.Item>
      <Form.Item className={('flex justify-center m-0')}>
        <Button className={('bg-[#1890FF]')}
                type='primary'
                onClick={onSubmitClick}
                disabled={paymentResult?.paymentTransaction.responseCode == '00' && paymentResult?.paymentTransaction.transactionStatus == '00'}
        >
          Đặt chỗ
        </Button>
      </Form.Item>
      <Form.Item className={('flex justify-center m-0')}>
        <Button className={('bg-[#1890FF] w-[200%]')} type='primary' onClick={onSubmitClick}>
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
