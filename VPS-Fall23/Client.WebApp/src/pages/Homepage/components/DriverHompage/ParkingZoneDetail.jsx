import { Alert, Badge, Button, Carousel, Descriptions, Image, Modal, Tabs, Tag, Typography, notification } from 'antd';
import useParkingZoneService from '../../../../services/parkingZoneService';
import { useEffect, useState } from 'react';
import BookingForm from './BookingForm';
import FeedBackForm from '@/pages/Homepage/components/DriverHompage/FeedBackForm.jsx';
import FeedbackList from './FeedbackList';



const ParkingZoneDetail = ({ parkingZone, isShow, onCloseCallback, defaultTab = '1' }) => {

  const parkingZoneService = useParkingZoneService();
  const [imageLinks, setImageLinks] = useState([]);
  const [freeSlots, setFreeSlots] = useState(parkingZone?.slots ?? 0);
  const [tab, setTab] = useState('1');
  useEffect(() => {
    setTab(defaultTab)
  }, [defaultTab])
  useEffect(() => {

    if (!parkingZone) return;
    parkingZoneService.getImageLink(parkingZone.id).then((res) => setImageLinks(res?.data));
  }, [parkingZone?.id]);
  const onGetFreeSlot = (parkingZoneId) => {
    parkingZoneService.getBookedSlot(parkingZoneId).then(res => setFreeSlots(parkingZone.slots - res.data));
  }
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
        key: 5,
        label: 'Số chỗ trống',
        children: (
          <Button
            onClick={() => onGetFreeSlot(parkingZone.id)}
          >{freeSlots ?? parkingZone.slots}</Button>
        ),
      },
    ];
  };
  const items = [
    {

      key: '1',
      label: 'Chi tiết',
      children: (<Descriptions
        bordered
        items={getDetailDescription()}
        size='small'
        column={{ xs: 1, sm: 1, md: 1, lg: 1, xl: 2, xxl: 2 }}
      />),
    },
    {
      key: '2',
      label: 'Xem đánh giá',
      children: <FeedbackList parkingZoneId={parkingZone?.id} />
    },
    {
      key: '3',
      label: 'Đặt chỗ',
      children: (<BookingForm
        parkingZone={parkingZone}

      ></BookingForm>),
    },
    {
      key: '4',
      label: 'Viết đánh giá',
      children: (<FeedBackForm
        parkingZoneId={parkingZone?.id}
      />),
    },
  ];
  const onChangeTabs = (key) => {
    setTab(key)
  };

  return (
    <>
      <Modal
        open={isShow}
        onCancel={onCloseCallback}
        zIndex={2000}
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
          <Tabs
            activeKey={tab}
            items={items}
            destroyInactiveTabPane={true}
            onChange={onChangeTabs} />
        </div>
      </Modal>

    </>
  );
};
export default ParkingZoneDetail;
