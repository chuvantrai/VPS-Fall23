import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Fragment } from 'react';
import { App as AntdApp, Spin } from 'antd';
import { routes } from '@/routes';
import { DefaultLayout } from '@/layouts';
import { ConfigProvider } from 'antd';
import { useSelector } from 'react-redux';
import { LoadingOutlined } from '@ant-design/icons';
import { getAccountJwtModel } from '@/helpers/index.js';
import Login from '@/pages/Login/index.js';

const antIcon = <LoadingOutlined style={{ fontSize: 24 }} spin />;

function App() {
  const { isLoading } = useSelector((state) => state.global);
  const account = getAccountJwtModel();
  // let filteredRoutes = routes
  //   .filter((route) => route.userRole?.includes(account?.RoleId) || !('userRole' in route));
  let filteredRoutes = routes
    .map((route) => {
      if (route.userRole?.includes(account?.RoleId) || !('userRole' in route)) {
        return route;
      }
      return { ...route, component: Login };
    });
  filteredRoutes = filteredRoutes.map((route) => ({
    ...route,
    subRoutes: route.subRoutes !== undefined ? route.subRoutes
      .map((subRoute) => {
        if (subRoute.userRole?.includes(account?.RoleId) || !('userRole' in subRoute)) {
          return subRoute;
        }
        return { ...subRoute, component: Login };
      }):undefined,
  }));
  return (
    <ConfigProvider>
      <AntdApp className='app'>
        <Spin spinning={isLoading} indicator={antIcon}>
          <Router>
            <Routes>
              {
                filteredRoutes.map((route, index) => {
                  const Page = route.component;
                  let Layout = DefaultLayout;
                  let subRoutes = [{ url: '', component: null }];
                  subRoutes = route.subRoutes;
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
