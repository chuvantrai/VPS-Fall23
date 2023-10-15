/* eslint-disable no-unused-vars */
/* eslint-disable react/prop-types */
import PropTypes from 'prop-types';
import classNames from 'classnames/bind';
import { Layout, Menu, theme } from 'antd';
const { Sider } = Layout;

import styles from './Sidebar.module.scss';

// eslint-disable-next-line no-unused-vars
const cx = classNames.bind(styles);

function Sidebar({ rowData, setContentState }) {
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  const handleMenuItem = (e) => {
    setContentState(e.key);
  };

  const items2 = rowData.map((data, index) => {
    const key = String(index + 1);
    return {
      key: `sub${key}`,
      label: `${data}`,
      children: new Array(4).fill(null).map((_, j) => {
        const subKey = index * 4 + j + 1;
        return {
          key: subKey,
          label: `option${subKey}`,
        };
      }),
    };
  });

  return (
    <Sider
      style={{
        background: colorBgContainer,
      }}
      width={246}
    >
      <Menu
        onClick={handleMenuItem}
        mode="inline"
        defaultSelectedKeys={['1']}
        defaultOpenKeys={['sub1']}
        items={items2}
      />
    </Sider>
  );
}

Sidebar.propTypes = {
  rowData: PropTypes.array.isRequired,
};

export default Sidebar;
