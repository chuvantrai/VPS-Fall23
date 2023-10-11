// import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames/bind';
import { Layout, Menu, theme } from 'antd';
const { Content, Sider } = Layout;

import styles from './Sidebar.module.scss';

const cx = classNames.bind(styles);

function Sidebar(props) {
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  const items2 = props.rowData.map((data, index) => {
    const key = String(index + 1);
    return {
      key: `sub${key}`,
      // icon: React.createElement(icon),
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
    <aside className={cx('wrapper w-full')}>
      <Layout
        style={{
          padding: '24px 0',
          background: colorBgContainer,
        }}
      >
        <Sider
          style={{
            background: colorBgContainer,
          }}
          width={200}
        >
          <Menu
            mode="inline"
            defaultSelectedKeys={['1']}
            defaultOpenKeys={['sub1']}
            style={{
              height: '100%',
            }}
            items={items2}
          />
        </Sider>
        <Content
          style={{
            padding: '0 24px',
            minHeight: 280,
          }}
        >
          Content
        </Content>
      </Layout>
    </aside>
  );
}

Sidebar.propTypes = {
  rowData: PropTypes.array.isRequired,
  // rowData: PropTypes.arrayOf(PropTypes.shape({
  //   name: ""
  // })).isRequired,
};

export default Sidebar;
