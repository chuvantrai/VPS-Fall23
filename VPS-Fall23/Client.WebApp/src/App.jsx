import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Fragment } from 'react';
import { App as AntdApp, Spin } from 'antd';
import { routes } from '@/routes';
import { DefaultLayout } from '@/layouts';
import { ConfigProvider } from 'antd';
import { useSelector } from 'react-redux';
import { LoadingOutlined } from '@ant-design/icons';

import Homepage from '@/pages/Homepage';
import UserProfile from './pages/Homepage/components/Content/UserProfile';
import ViewListParkingZone from './pages/Homepage/components/Content/ViewListParkingZone';

const antIcon = <LoadingOutlined style={{ fontSize: 24 }} spin />;
function App() {
  const { isLoading } = useSelector((state) => state.global);
  return (
    <ConfigProvider>
      <AntdApp className="app">
        <Spin spinning={isLoading} indicator={antIcon}>
          <Router>
            <Routes>
              {routes.map((route, index) => {
                const Page = route.component;
                let Layout = DefaultLayout;
                let subRoutes = [{ url: '', component: null }];
                subRoutes = route.subRoutes;
                console.log();
                if (route.layout) {
                  Layout = route.layout;
                } else if (route.layout === null) {
                  Layout = Fragment;
                }

                return (
                  <Route
                    key={index}
                    path={route.path}
                    element={
                      <Layout>
                        <Page></Page>
                      </Layout>
                    }
                  >
                    {subRoutes !== undefined &&
                      subRoutes.map((subRoute, index) => {
                        return (
                          <Route
                            key={index}
                            path={subRoute.url}
                            element={<subRoute.component></subRoute.component>}
                          ></Route>
                        );
                      })}
                  </Route>
                );
              })}
            </Routes>
          </Router>
        </Spin>
      </AntdApp>
    </ConfigProvider>
  );
}

export default App;
