import { Button, Col, Row, notification } from 'antd';
import styles from './HeaderCenter.module.scss';
import classNames from 'classnames/bind';
import { useCallback, useState } from 'react';
import useParkingZoneService from '@/services/parkingZoneService';
import { setFoundedParkingZones } from '@/stores/parkingZones/parkingZone.store';
import store from '@/stores/index';
import AddressCascader from '@/components/cascader/AddressCascader';

const cx = classNames.bind(styles);

const HeaderCenter = () => {
  const [selectedAddress, setSelectedAddress] = useState(null);
  const parkingZoneService = useParkingZoneService();
  const addressCascaderProps = {
    style: { width: '100%' },
    placeholder: 'Chọn địa điểm bạn muốn đến',
  };
  const onCascaderChange = useCallback((value, selectedOptions) => {
    setSelectedAddress(selectedOptions ? selectedOptions[selectedOptions.length - 1] : null);
  }, []);
  const onSearch = () => {
    if (!selectedAddress) {
      notification.error({
        message: 'Vui lòng chọn địa điểm bạn muốn tới',
        description: 'Vui lòng chọn địa điểm bạn muốn tới trong ô lựa chọn phía trên',
      });
      return;
    }
    parkingZoneService.getByAddress(selectedAddress.id, selectedAddress.type).then((res) => {
      store.dispatch(setFoundedParkingZones({ listFounded: res?.data ?? [] }));
    });
  };

  return (
    <div className={cx('wrapper')}>
      <Row gutter={2}>
        <Col md={20} sm={24}>
          <AddressCascader cascaderProps={addressCascaderProps} onCascaderChangeCallback={onCascaderChange} />
        </Col>
        <Col md={4}>
          <Button onClick={onSearch}>Tìm kiếm</Button>
        </Col>
      </Row>
    </div>
  );
};
export default HeaderCenter;


