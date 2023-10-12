import Sidebar from '@/layouts/components/Sidebar/Sidebar';
import UserProfile from './Content/UserProfile';
import { Layout, Breadcrumb, theme } from 'antd';
import { useState } from 'react';
import ViewListParkingZone from './Content/ViewListParkingZone';

const { Content } = Layout;

function HomepageAdmin() {
  const [contentState, setContentState] = useState('1');
  const {
    token: { colorBgContainer },
  } = theme.useToken();
  const rowData = ['user', 'manager'];

  return (
    <Layout className="min-h-screen">
      <Sidebar rowData={rowData} setContentState={setContentState} />
      <Layout
        style={{
          padding: '0 24px 24px',
        }}
      >
        <Breadcrumb
          style={{
            margin: '16px 0',
          }}
        >
          <Breadcrumb.Item>Home</Breadcrumb.Item>
          <Breadcrumb.Item>List</Breadcrumb.Item>
          <Breadcrumb.Item>App</Breadcrumb.Item>
        </Breadcrumb>
        <Content
          style={{
            padding: 24,
            margin: 0,
            minHeight: 280,
            background: colorBgContainer,
          }}
        >
          {contentState === '1' && <UserProfile></UserProfile>}
          {contentState === '2' && <ViewListParkingZone></ViewListParkingZone>}
        </Content>
      </Layout>
    </Layout>
  );
}

export default HomepageAdmin;
