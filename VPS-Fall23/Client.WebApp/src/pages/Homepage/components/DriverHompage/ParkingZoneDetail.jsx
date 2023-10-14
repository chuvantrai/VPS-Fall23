import { Alert, Badge, Carousel, Descriptions, Image, Modal, Typography, notification } from 'antd';
import useParkingZoneService from '../../../../services/parkingZoneService';
import { useEffect, useState } from 'react';
import BookingForm from './BookingForm';
import useParkingTransactionService from '../../../../services/parkingTransactionSerivce';
import { useSelector } from 'react-redux';
import { setShowBookingForm } from '../../../../stores/parkingZones/parkingZone.store';
import store from '../../../../stores';
const { Text } = Typography;

const ParkingZoneDetail = ({ parkingZone, isShow, onCloseCallback }) => {
  const parkingZoneService = useParkingZoneService();
  const parkingTransactionService = useParkingTransactionService();
  const [imageLinks, setImageLinks] = useState([]);
  const { isShowBookingForm } = useSelector((store) => store.parkingZone);
  console.log(isShowBookingForm);
  useEffect(() => {
    if (!parkingZone) return;
    parkingZoneService.getImageLink(parkingZone.id).then((res) => setImageLinks(res?.data));
  }, [parkingZone?.id]);
  const getDetailDescription = () => {
    if (!parkingZone) return [];
    return [
      {
        key: 1,
        label: 'Địa chỉ',
        children: parkingZone.detailAddress,
      },
      {
        key: 2,
        label: 'Xã/Phường',
        children: parkingZone.commune.name,
      },
      {
        key: 3,
        label: 'Quận/Huyện',
        children: parkingZone.commune.district.name,
      },
      {
        key: 4,
        label: 'Tỉnh/Thành phố',
        children: parkingZone.commune.district.city.name,
      },
      {
        key: 5,
        label: 'Giá thành mỗi giờ (VNĐ)',
        children: parkingZone.pricePerHour ?? 0,
      },
      {
        key: 6,
        label: 'Giá khi đỗ quá giờ (VNĐ)',
        children: parkingZone.priceOverTimePerHour ?? 0,
      },
      {
        key: 4,
        label: 'Tình trạng hoạt động',
        children: <Badge status="processing" text="Đang hoạt động" />,
      },
      {
        key: 5,
        label: 'Số chỗ trống',
        children: (
          <Alert
            style={{ padding: 0, textAlign: 'center' }}
            type="error"
            description={
              <Text type="danger" style={{}}>
                {50}
              </Text>
            }
          />
        ),
      },
    ];
  };
  const showBookingForm = () => {
    store.dispatch(setShowBookingForm({ isShowBookingForm: false }));
    store.dispatch(setShowBookingForm({ isShowBookingForm: true }));
  };
  const closeBookingForm = () => {
    store.dispatch(setShowBookingForm({ isShowBookingForm: false }));
  };
  const onConfirmBooking = (parkingTransaction) => {
    parkingTransactionService.bookingSlot(parkingTransaction).then((res) => {
      notification.success('Đặt chỗ thành công');
      store.dispatch(setShowBookingForm({ isShowBookingForm: false }));
    });
  };
  return (
    <>
      <Modal
        open={isShow}
        onCancel={onCloseCallback}
        onOk={showBookingForm}
        okText="Đặt vé"
        cancelText="Thoát"
        okButtonProps={{
          style: {
            backgroundColor: '#1677ff',
          },
        }}
        closable={true}
        title={parkingZone?.name}
        width={'40vw'}
      >
        <Image.PreviewGroup>
          <Carousel autoplay dotPosition="top" style={{}}>
            {imageLinks.map((val, index) => {
              return <Image key={index} src={val}></Image>;
            })}
          </Carousel>
        </Image.PreviewGroup>
        <Descriptions
          title="Chi tiết"
          bordered
          items={getDetailDescription()}
          size="small"
          column={{ xs: 1, sm: 1, md: 1, lg: 1, xl: 2, xxl: 2 }}
        />
      </Modal>
      <BookingForm
        isShow={isShowBookingForm}
        parkingZone={parkingZone}
        onCloseCallback={closeBookingForm}
        onSubmitCallback={onConfirmBooking}
      ></BookingForm>
    </>
  );
};
export default ParkingZoneDetail;
