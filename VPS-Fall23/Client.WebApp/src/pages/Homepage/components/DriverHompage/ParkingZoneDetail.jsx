import { Alert, Badge, Carousel, Descriptions, Image, Modal, Tag, Typography, notification } from 'antd';
import useParkingZoneService from '../../../../services/parkingZoneService';
import { useEffect, useState } from 'react';
import BookingForm from './BookingForm';
import { useSelector } from 'react-redux';
import { setShowBookingForm } from '../../../../stores/parkingZones/parkingZone.store';
import store from '../../../../stores';
import FeedBackForm from '@/pages/Homepage/components/DriverHompage/FeedBackForm.jsx';

const { Text } = Typography;

const ParkingZoneDetail = ({ parkingZone, isShow, onCloseCallback }) => {
  const parkingZoneService = useParkingZoneService();

  const [imageLinks, setImageLinks] = useState([]);
  const { isShowBookingForm } = useSelector((store) => store.parkingZone);
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
        children: <Badge status="success" text="Đang hoạt động" />,
      },
      {
        key: 5,
        label: 'Số chỗ trống',
        children: (
          <Tag color="processing">{parkingZone.slots}</Tag>
        ),
      },
    ];
  };
  const showBookingForm = () => {
    store.dispatch(setShowBookingForm({ isShowBookingForm: false }));
    store.dispatch(setShowBookingForm({ isShowBookingForm: true }));
  };

  return (
    <>
      <Modal
        open={isShow}
        onCancel={onCloseCallback}
        onOk={showBookingForm}
        zIndex={2000}
        okText="Đặt vé"
        cancelText="Thoát"
        okButtonProps={{
          style: {
            display: 'none',
          },
        }}
        cancelButtonProps={{
          style: {
            display: 'none',
          },
        }}
        closable={true}
        title={parkingZone?.name}
      >
        <Image.PreviewGroup>
          <Carousel autoplay dotPosition='top' style={{}}>
            {imageLinks.map((val, index) => {
              return <Image key={index} src={val}></Image>;
            })}
          </Carousel>
        </Image.PreviewGroup>
        <div className={('pt-[10px]')}>
          <Tabs defaultActiveKey='1' items={items} onChange={onChangeTabs} />
        </div>
      </Modal>
      <BookingForm
        isShow={isShowBookingForm}
        parkingZone={parkingZone}
      ></BookingForm>
    </>
  );
};
export default ParkingZoneDetail;
