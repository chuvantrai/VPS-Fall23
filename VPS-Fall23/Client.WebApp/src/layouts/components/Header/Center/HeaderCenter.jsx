import { Cascader, Col, Row } from 'antd';
import styles from './HeaderCenter.module.scss';
import classNames from 'classnames/bind';
import { useEffect, useState } from 'react';
import useAddressServices from '@/services/addressServices';

const cx = classNames.bind(styles);
const getDataTypes = {
  city: {
    getFuncName: 'getDistricts',
    childType: 'district',
    isLeaf: false,
  },
  district: {
    getFuncName: 'getCommunes',
    childType: 'commune',
    isLeaf: true,
  },
};
const fieldNames = {
  label: 'name',
  value: 'id',
  children: 'children',
};

const HeaderCenter = () => {
  return (
    <div className={cx('wrapper')}>
      <Row gutter={2}>
        <Col md={24} sm={24}>
          <Cascader style={{ width: '100%' }} placeholder="Chọn địa điểm bạn muốn đến" changeOnSelect />
        </Col>
      </Row>
    </div>
  );
};
export default HeaderCenter;
