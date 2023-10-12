import { useState } from 'react';
import classNames from 'classnames/bind';
import PropTypes from 'prop-types';
import { Layout, Breadcrumb, theme } from 'antd';

import styles from './DefaultLayout.module.scss';
import Header from '@/layouts/components/Header';
import Sidebar from '@/layouts/components/Sidebar';
import UserProfile from '@/pages/Homepage/components/Content/UserProfile';
import ViewListParkingZone from '@/pages/Homepage/components/Content/ViewListParkingZone';
import RegisterParkingZone from '@/pages/RegisterParkingZone/RegisterParkingZone';

const { Content } = Layout;

const cx = classNames.bind(styles);

function DefaultLayout() {
  const {
    token: { colorBgContainer },
  } = theme.useToken();
  const rowData = [
    {
      label: 'User',
      options: ['Profile', 'Test'],
    },
    {
      label: 'Manage',
      options: ['View Parking Zone List'],
    },
  ];

  const [contentState, setContentState] = useState('1');

  return (
    <Layout className={cx('wrapper w-full min-h-screen')}>
      <Header />
      <Layout>
        <Sidebar rowData={rowData} setContentState={setContentState} />
        <Layout className="bg-[#f0f2f5]">
          <div className="w-full bg-white py-[16px] px-[24px] mb-[20px]">
            <Breadcrumb>
              <Breadcrumb.Item>Home</Breadcrumb.Item>
              <Breadcrumb.Item>List</Breadcrumb.Item>
              <Breadcrumb.Item>App</Breadcrumb.Item>
            </Breadcrumb>
            <div className="w-[808px] h-11 justify-start items-center gap-4 inline-flex mt-[8px] mb-[8px]">
              <div className="justify-start items-center gap-3 flex">
                <div className="text-black text-opacity-90 text-[22px] font-medium font-['Roboto'] leading-7">
                  Form Đăng ký bãi giữ xe
                </div>
              </div>
            </div>
            <div className="w-[1146px] text-zinc-800 text-[13px] font-normal font-['Roboto'] leading-[17.03px]">
              Điền form dưới đây để đăng ký bãi giữ xe mới
            </div>
          </div>
          <div
            className="h-fit bg-[#f0f2f5]"
            style={{
              padding: '0 24px',
            }}
          >
            <Content
              className="flex justify-center items-center"
              style={{
                padding: '24px 0px',
                margin: 0,
                background: colorBgContainer,
              }}
            >
              {contentState === '1' && <UserProfile></UserProfile>}
              {contentState === '2' && <ViewListParkingZone></ViewListParkingZone>}
              {contentState === '3' && <RegisterParkingZone />}
            </Content>
          </div>
        </Layout>
      </Layout>
    </Layout>
  );
}

DefaultLayout.propTypes = {
  children: PropTypes.node.isRequired,
};

export default DefaultLayout;
