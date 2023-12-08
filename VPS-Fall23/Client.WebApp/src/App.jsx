import { BrowserRouter as Router, Routes, Route, useLocation, BrowserRouter } from 'react-router-dom';
import { App as AntdApp, Spin } from 'antd';
import { ConfigProvider } from 'antd';
import { useSelector } from 'react-redux';
import { LoadingOutlined } from '@ant-design/icons';
import RouteWithNoLayout from './layouts/RouteWithNoLayout';
import RouteWithLayout from './layouts/RouteWithLayout';
const antIcon = <LoadingOutlined style={{ fontSize: 24 }} spin />;

function App() {
  const { isLoading } = useSelector((state) => state.global);
  return (
    <ConfigProvider>
      <AntdApp className="app">
        <Spin spinning={isLoading} indicator={antIcon} style={{ zIndex: 10000000 }}>
          <BrowserRouter>
            <RouteWithNoLayout></RouteWithNoLayout>
            <RouteWithLayout></RouteWithLayout>

          </BrowserRouter>
        </Spin>
      </AntdApp>
    </ConfigProvider>
  );
}

export default App;