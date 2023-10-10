import classNames from 'classnames/bind';

import styles from './Sidebar.module.scss';
import Sider from 'antd/es/layout/Sider';

const cx = classNames.bind(styles);

function Sidebar() {
  return (
    <Sider className={cx('wrapper')}>
      <h2>Sidebar</h2>
    </Sider>
  );
}

export default Sidebar;
