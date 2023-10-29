import { PicCenterOutlined, EnvironmentOutlined } from '@ant-design/icons';
import { Button, Descriptions, Input, List, Popover, notification } from 'antd';
import { useSelector } from 'react-redux';
import ButtonGroup from 'antd/es/button/button-group';
import { useCallback, useState } from 'react';
import ParkingZoneDetail from './ParkingZoneDetail';
const viewOnGoogleMapCallback = (parkingZone) => {
  window.open(`https://maps.google.com/?q=${parkingZone.lat},${parkingZone.lng}`, '_blank');
};
const FoundedParkingZone = ({ viewOnThisMapCallback }) => {
  const { listFounded } = useSelector((state) => state.parkingZone);
  const [detailFormInfo, setDetailFormInfo] = useState({ parkingZone: null, isShow: false, defaultTab: '1' });
  const [listFoundedSearch, setListFoundedSearch] = useState();
  const viewDetailInfo = (parkingZone, defaultTab = '1') => {
    setDetailFormInfo({ defaultTab: defaultTab, parkingZone: parkingZone, isShow: true });
  };
  const onDetailCloseCallback = useCallback(() => {
    setDetailFormInfo({ defaultTab: '1', parkingZone: null, isShow: false });
  }, []);

  const getDescriptionItem = (parkingZone) => [
    {
      key: 1,
      span: 2,
      label: <Button onClick={() => viewDetailInfo(parkingZone, '3')} type="primary" danger>Đặt vé</Button>,
      children: (<ButtonGroup>
        <Button onClick={() => viewDetailInfo(parkingZone)}>{parkingZone.name}</Button>

      </ButtonGroup>
      )
    },
    {
      key: 2,
      label: 'Số lượng xe tối đa',
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

    </>
  );
  const getListFounded = () => {
    if (!listFoundedSearch) return listFounded;
    return listFounded.filter(val => {
      return val.detailAddress.toLowerCase().trim().includes(listFoundedSearch.toLowerCase().trim());
    })
  }
  const getFoundedParkingZonePopupContent = () => {
    return (
      <>
        <Input
          type='search'
          placeholder='Nhập để tìm nhà xe theo địa chỉ cụ thể'
          onChange={({ target }) => setListFoundedSearch(target.value)}
        >

        </Input>
        <List
          pagination={{
            position: 'top',
            align: 'center',
            size: 'small',
            pageSize: 2,
            total: getListFounded().length,
          }}
          dataSource={getListFounded()}
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
      </>
    );
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

      ></ParkingZoneDetail>
    </>

  );
};
export default FoundedParkingZone;
