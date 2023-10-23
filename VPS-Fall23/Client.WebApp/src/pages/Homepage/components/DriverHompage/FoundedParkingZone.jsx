import { PicCenterOutlined, EnvironmentOutlined } from '@ant-design/icons';
import { Button, Descriptions, List, Popover, notification } from 'antd';
import { useSelector } from 'react-redux';
import ButtonGroup from 'antd/es/button/button-group';
import useParkingZoneService from '../../../../services/parkingZoneService';
import { useCallback, useState } from 'react';
import ParkingZoneDetail from './ParkingZoneDetail';
import store from '../../../../stores';
import { setShowBookingForm } from '../../../../stores/parkingZones/parkingZone.store';
const viewOnGoogleMapCallback = (parkingZone) => {
  window.open(`https://maps.google.com/?q=${parkingZone.lat},${parkingZone.lng}`, '_blank');
};
const FoundedParkingZone = ({ viewOnThisMapCallback }) => {
  const { listFounded } = useSelector((state) => state.parkingZone);
  const [detailFormInfo, setDetailFormInfo] = useState({ parkingZone: null, isShow: false });
  const parkingZoneService = useParkingZoneService();
  const viewDetailInfo = (parkingZone) => {
    setDetailFormInfo({ parkingZone: parkingZone, isShow: true });
  };
  const onDetailCloseCallback = useCallback(() => {
    setDetailFormInfo({ parkingZone: null, isShow: false });
  }, []);

  const getDescriptionItem = (parkingZone) => [
    {
      key: 1,
      span: 2,
      label: <Button onClick={() => bookingCallback(parkingZone)} type="primary" danger>Đặt vé</Button>,
      children: (<ButtonGroup>
        <Button onClick={() => viewDetailInfo(parkingZone)}>{parkingZone.name}</Button>

      </ButtonGroup>
      )
    },
    {
      key: 2,
      label: 'Số lượng trống',
      children: parkingZone.slots ?? 0,
    },
    {
      key: 3,
      label: 'Giá/giờ',
      children: <p>{parkingZone.pricePerHour} VNĐ</p>,
    },
    {
      key: 4,

      label: <>Địa chỉ    <Button onClick={() => viewOnThisMapCallback(parkingZone)} icon={<EnvironmentOutlined />}></Button></>,
      children: <Button onClick={() => viewOnGoogleMapCallback(parkingZone)}> {parkingZone.detailAddress}</Button>,
    },
  ];
  const getDesciptionTitle = (parkingZone) => (
    <>
      {/* <ButtonGroup>
        <Button onClick={() => viewDetailInfo(parkingZone)}>{parkingZone.name}</Button>
        <Button onClick={() => bookingCallback(parkingZone)} type="primary" danger>
          Đặt vé
        </Button>
      </ButtonGroup> */}
    </>
  );
  const getFoundedParkingZonePopupContent = () => {
    return (
      <List
        pagination={{
          position: 'top',
          align: 'center',
          size: 'small',
          pageSize: 2,
          total: listFounded.length,
        }}
        dataSource={listFounded}
        renderItem={(val, index) => {
          return (
            <List.Item key={index}>
              <Descriptions
                title={getDesciptionTitle(val)}
                size="small"
                items={getDescriptionItem(val)}
                bordered={true}
                column={2}
              />
            </List.Item>
          );
        }}
      ></List>
    );
  };

  const bookingCallback = (parkingZone) => {
    setDetailFormInfo({ ...detailFormInfo, parkingZone: parkingZone });
    store.dispatch(setShowBookingForm({ isShowBookingForm: true }));
  };
  return (
    <>
      <Popover
        trigger={'click'}
        content={getFoundedParkingZonePopupContent}
        placement="bottomLeft"
        title="Danh sách nhà xe đã tìm được"
      >
        <Button type="primary" danger icon={<PicCenterOutlined />}></Button>
      </Popover>
      <ParkingZoneDetail
        {...detailFormInfo}
        onCloseCallback={onDetailCloseCallback}
        bookingCallback={bookingCallback}
      ></ParkingZoneDetail>
    </>
  );
};
export default FoundedParkingZone;
