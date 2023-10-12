import classNames from 'classnames/bind';
import { Button, Typography } from 'antd';

import styles from './HeaderRight.module.scss';

const cs = classNames.bind(styles);
const { Text } = Typography;

const HeaderRight = () => {
  return (
    <div className={cs('wrapper')}>
      <Text>
        Bạn muốn quản lý nhà xe của bạn?
        <Button
          type="primary"
          htmlType="button"
          style={{
            backgroundColor: '#1677ff',
            marginLeft: '10px',
          }}
        >
          Đăng ký
        </Button>
      </Text>
    </div>
  );
};
export default HeaderRight;

