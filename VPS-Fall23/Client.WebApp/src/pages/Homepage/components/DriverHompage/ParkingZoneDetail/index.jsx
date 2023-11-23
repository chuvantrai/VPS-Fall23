import { Modal } from 'antd';
import useParkingZoneService from '@/services/parkingZoneService.js';
import { useEffect, useState } from 'react';
import ParkingZoneDetailTabs from './Tabs.jsx';
import ParkingZoneDetailCarousel from './Carousel.jsx';


const ParkingZoneDetail = ({ parkingZone, isShow, onCloseCallback }) => {
  const parkingZoneService = useParkingZoneService();
  const [imageLinks, setImageLinks] = useState([]);
  useEffect(() => {
    if (!parkingZone) return;
    parkingZoneService.getImageLink(parkingZone.id).then((res) => setImageLinks(res?.data));
  }, [parkingZone?.id]);
  const okButtonProps = {
    style: {
      display: 'none',
    },
  }
  const cancelButtonProps = {
    style: {
      display: 'none',
    },
  }
  return (
    <Modal
      open={isShow}
      onCancel={onCloseCallback}
      zIndex={1}
      centered={true}
      okButtonProps={okButtonProps}
      cancelButtonProps={cancelButtonProps}
      closable={true}
      title={parkingZone?.name}
      destroyOnClose={true}
    >
      <ParkingZoneDetailCarousel imageLinks={imageLinks} />
      <ParkingZoneDetailTabs parkingZone={parkingZone} />
    </Modal>
  );
};
export default ParkingZoneDetail;
