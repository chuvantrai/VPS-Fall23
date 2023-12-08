import classNames from 'classnames/bind';
import { Header as AntdHeader } from 'antd/es/layout/layout';
import styles from './Header.module.scss';
import { Col, Menu, Row } from 'antd';
import HeaderLeft from '@/layouts/components/Header/Left';
import HeaderRight from '@/layouts/components/Header/Right';
import HeaderCenter from './Center/HeaderCenter';
const cx = classNames.bind(styles);

function Header() {
  return (
    <AntdHeader className={cx('nav')}    >
      <Row className={cx('nav-row')}>
        <Col md={8} sm={0} xs={0} className={cx('nav-left')}>
          <HeaderLeft></HeaderLeft>
        </Col>
        <Col md={8} sm={24} style={{ textAlign: "center" }}>
          <HeaderCenter></HeaderCenter>
        </Col>
        <Col md={8} sm={24}>
          <HeaderRight></HeaderRight>
        </Col>
      </Row>

    </AntdHeader>
  );
}

export default Header;
