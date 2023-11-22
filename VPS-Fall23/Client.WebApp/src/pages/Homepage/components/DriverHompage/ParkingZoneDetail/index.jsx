import { Button, Carousel, Descriptions, Image, Modal, Tabs, notification, Tooltip } from 'antd';
import useParkingZoneService from '@/services/parkingZoneService.js';
import { useEffect, useState } from 'react';
import BookingForm from './Booking/BookingForm.jsx';
import FeedBackForm from './FeedBackForm.jsx';
import FeedbackList from './FeedbackList.jsx';
import { BookOutlined, BookTwoTone } from '@ant-design/icons';
import { getListBookmarkParkingZone, setListBookmarkParkingZone } from '@/helpers/index.js';
import Detail from './Detail.jsx';


const ParkingZoneDetail = ({ parkingZone, isShow, onCloseCallback, defaultTab = '1' }) => {

  const parkingZoneService = useParkingZoneService();
  const [imageLinks, setImageLinks] = useState([]);

  const [tab, setTab] = useState('1');
  useEffect(() => {
    setTab(defaultTab);
  }, [defaultTab]);
  useEffect(() => {
    if (isShow && parkingZone !== undefined && parkingZone.id) {
      const arrayBookmarkPzId = getListBookmarkParkingZone() ?? [];
      setIsBookmark(arrayBookmarkPzId.includes(parkingZone?.id));
    }
    if (!parkingZone) return;
    parkingZoneService.getImageLink(parkingZone.id).then((res) => setImageLinks(res?.data));
  }, [parkingZone?.id]);

  const items = [
    {

      key: '1',
      label: 'Chi tiết',
      children: (<Detail parkingZone={parkingZone}></Detail>),
    },
    {
      key: '2',
      label: 'Xem đánh giá',
      children: <FeedbackList parkingZoneId={parkingZone?.id} />,
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
    setTab(key);
  };

  const [isBookmark, setIsBookmark] = useState(false);
  const ClickBookMark = () => {
    let arrayBookmarkPzId = getListBookmarkParkingZone() ?? [];
    if (!isBookmark) {
      arrayBookmarkPzId.push(parkingZone.id);
      setIsBookmark(true);
      notification.success({
        message: 'Lưu thành công',
      });
    } else {
      arrayBookmarkPzId = arrayBookmarkPzId.filter(item => item !== parkingZone.id);
      setIsBookmark(false);
      notification.success({
        message: 'Bỏ lưu thành công',
      });
    }
    setListBookmarkParkingZone(arrayBookmarkPzId);
  };
  const operations = (
    <>
      <Tooltip placement='bottom' title={isBookmark ? 'Bỏ lưu bãi đỗ xe' : 'Lưu bãi đỗ xe'} zIndex={10000}>
        <Button icon={isBookmark ? <BookTwoTone /> : <BookOutlined />} onClick={ClickBookMark}></Button>
      </Tooltip>
    </>);

  return (
    <Modal
      open={isShow}
      onCancel={onCloseCallback}
      zIndex={1}
      centered={true}
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
      destroyOnClose={true}
    >

      <Image.PreviewGroup>
        <Carousel adaptiveHeight={true} autoplay dotPosition='top'>
          {imageLinks.map((val, index) => {
            return <Image width={'100%'} style={{ objectFit: "cover" }} key={index} src={val}></Image>;
          })}
        </Carousel>
      </Image.PreviewGroup>

      <div className={('pt-[10px]')}>
        <Tabs
          activeKey={tab}
          items={items}
          destroyInactiveTabPane={true}
          onChange={onChangeTabs}
          tabBarExtraContent={operations} />
      </div>
    </Modal>
  );
};
export default ParkingZoneDetail;
