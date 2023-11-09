import { PicCenterOutlined, EnvironmentOutlined, BookFilled } from '@ant-design/icons';
import { Button, Descriptions, Input, List, Popover } from 'antd';
import { useSelector } from 'react-redux';
import ButtonGroup from 'antd/es/button/button-group';
import { useCallback, useEffect, useState } from 'react';
import ParkingZoneDetail from './ParkingZoneDetail';
import { getListBookmarkParkingZone } from '../../../../helpers/index.js';
import useParkingZoneService from '../../../../services/parkingZoneService.js';
import { setListDataBookmark } from '../../../../stores/parkingZones/parkingZone.store.js';
import store from '../../../../stores/index.jsx';

const viewOnGoogleMapCallback = (parkingZone) => {
  window.open(`https://maps.google.com/?q=${parkingZone.lat},${parkingZone.lng}`, '_blank');
};
const FoundedParkingZone = ({ viewOnThisMapCallback }) => {
  const { listFounded } = useSelector((state) => state.parkingZone);
  const { listDataBookmark } = useSelector((state) => state.parkingZone);
  const [visiblePopover, setVisiblePopover] = useState(false);
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
      label: <Button onClick={() => viewDetailInfo(parkingZone, '3')} type='primary' danger>Đặt vé</Button>,
      children: (<ButtonGroup>
          <Button onClick={() => viewDetailInfo(parkingZone)}>{parkingZone.name}</Button>

        </ButtonGroup>
      ),
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

      label: <>Địa chỉ <Button onClick={() => viewOnThisMapCallback(parkingZone)}
                               icon={<EnvironmentOutlined />}></Button></>,
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
      return val.detailAddress.toLowerCase().trim().includes(listFoundedSearch.toLowerCase().trim())
        || val.name.toLowerCase().trim().includes(listFoundedSearch.toLowerCase().trim());
    });
  };
  const parkingZoneService = useParkingZoneService();
  const getListDataBookmark = () => {
    if(!visiblePopover){
      let arrayBookmarkPzId = getListBookmarkParkingZone() ?? [];
      parkingZoneService.GetParkingZonesByParkingZoneIds(arrayBookmarkPzId)
        .then((data) => {
          store.dispatch(setListDataBookmark({ listDataBookmark: data.data ?? [] }));
        });
    }
    setVisiblePopover(!visiblePopover);
  };
  const getFoundedParkingZonePopupContent = () => {
    return (
      <>
        <Input
          type='search'
          placeholder='Nhập để tìm nhà xe...'
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
                  size='small'
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
  const getListBookmark = () => {
    return (
      <>
        <List
          pagination={{
            position: 'top',
            align: 'center',
            size: 'small',
            pageSize: 2,
            total: listDataBookmark.length,
          }}
          dataSource={listDataBookmark}
          renderItem={(val, index) => {
            return (
              <List.Item key={index}>
                <Descriptions
                  title={getDesciptionTitle(val)}
                  size='small'
                  items={getDescriptionItem(val)}
                  bordered={true}
                  column={2}
                />
              </List.Item>
            );
          }}
        ></List>
      </>);
  };
  return (
    <>
      <div className={'flex'}>
        <Popover
          trigger={'click'}
          content={getFoundedParkingZonePopupContent}
          placement='bottomLeft'
          title='Danh sách nhà xe đã tìm được'
        >
          <Button type='primary' danger icon={<PicCenterOutlined />}></Button>
        </Popover>
        <Popover
          trigger={'click'}
          content={getListBookmark}
          placement='bottomLeft'
          title='Danh sách nhà xe đã được lưu'
          className={'ml-4'}
        >
          <Button onClick={getListDataBookmark} type='primary' icon={<BookFilled />}></Button>
        </Popover>
      </div>
      <ParkingZoneDetail
        {...detailFormInfo}
        onCloseCallback={onDetailCloseCallback}

      ></ParkingZoneDetail>
    </>

  );
};
export default FoundedParkingZone;
