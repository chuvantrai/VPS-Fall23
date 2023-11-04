import { Layout, theme } from 'antd';
import { Outlet } from 'react-router-dom';

const { Content } = Layout;

function ContentLayout({ description }) {
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  return (
    <Layout className="bg-[#f0f2f5]">
      <Content
        className="flex justify-center items-center"
        style={{
          padding: '2%',
          background: colorBgContainer,
        }}
      >
        <Outlet></Outlet>
      </Content>
    </Layout>
  );
}

export default ContentLayout;
