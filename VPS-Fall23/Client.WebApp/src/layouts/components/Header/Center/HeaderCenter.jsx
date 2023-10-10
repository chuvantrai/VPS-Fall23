import { Cascader, Col, Row } from 'antd';
import styles from './HeaderCenter.module.scss';
import classNames from 'classnames/bind';

const cx = classNames.bind(styles);
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
