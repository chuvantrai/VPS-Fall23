import { useState } from 'react';
import classNames from 'classnames/bind';
import PropTypes from 'prop-types';

import styles from './DefaultLayout.module.scss';
import Header from '@/layouts/components/Header';
import Sidebar from '@/layouts/components/Sidebar';
import { theme, Layout } from 'antd';
import UserProfile from '../../pages/Homepage/components/Content/UserProfile';
import ViewListParkingZone from '../../pages/Homepage/components/Content/ViewListParkingZone';
const { Content } = Layout;

const cx = classNames.bind(styles);

function DefaultLayout() {
  const {
    token: { colorBgContainer },
  } = theme.useToken();
  const rowData = ["user", "manager"];

  const [contentState, setContentState] = useState("1");

  return (
    <div className={cx('wrapper')}>
      <Header />
      <Layout>
        <Content
          style={{
            padding: '0 50px',
          }}>
          <Layout
            style={{
              padding: '24px 0',
              background: colorBgContainer,
            }}
          >
            <Sidebar rowData={rowData} setContentState={setContentState}></Sidebar>
            <Content
              style={{
                padding: '0 24px',
                minHeight: 280,
              }}
            >
              {contentState === "1" && <UserProfile></UserProfile>}
              {contentState === "2" && <ViewListParkingZone></ViewListParkingZone>}

            </Content>
          </Layout>
        </Content>
      </Layout>

    </div>
  );
}

DefaultLayout.propTypes = {
  children: PropTypes.node.isRequired,
};

export default DefaultLayout;
