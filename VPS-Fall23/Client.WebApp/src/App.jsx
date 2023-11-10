import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { App as AntdApp, Layout, Spin } from 'antd';
import { ConfigProvider } from 'antd';
import { useSelector } from 'react-redux';
import { LoadingOutlined } from '@ant-design/icons';
import getAccountJwtModel from './helpers/getAccountJwtModel';
import classNames from 'classnames/bind';
import styles from './App.module.scss';
import routes from './routes';
import guidGenerator from './helpers/guidGenerator';
const antIcon = <LoadingOutlined style={{ fontSize: 24 }} spin />;
const cx = classNames.bind(styles);
function App() {
  const { isLoading } = useSelector((state) => state.global);
  const account = getAccountJwtModel();
  const userRoutesConfig = routes.main[account?.RoleId ?? 0];

  const getRouteComponent = (parentRoutes, parentPath) => {
    return parentRoutes.map((parentRoute, index) => {
      return parentRoute.component ? (
        <Route
          path={parentPath ? `${parentPath}/${parentRoute.path}` : parentRoute.path}
          element={<parentRoute.component />}
          key={guidGenerator()}
        >
          {parentRoute.children ? getRouteComponent(parentRoute.children) : <></>}
        </Route>
      ) : (
        <Route key={guidGenerator()}>
          {parentRoute.children ? getRouteComponent(parentRoute.children, parentRoute.path) : <></>}
        </Route>
      );
    });
  };
  return (
    <ConfigProvider>
      <AntdApp className="app">
        <Spin spinning={isLoading} indicator={antIcon} style={{ zIndex: 10000000 }}>
          <Router>
            <Routes>
              {routes.noLayout.map((route, index) => {
                return <Route key={index} path={route.path} element={<route.component />} />;
              })}
            </Routes>
            <Layout className={cx('wrapper w-full min-h-screen')}>
              {userRoutesConfig.header !== null ? <userRoutesConfig.header /> : <></>}
              <Routes>
                <Route key={guidGenerator()} path="/" element={<userRoutesConfig.layout />}>
                  {getRouteComponent(userRoutesConfig.routes)}
                </Route>
              </Routes>
              {userRoutesConfig.footer != null ? <userRoutesConfig.footer /> : <></>}
            </Layout>
          </Router>
        </Spin>
      </AntdApp>
    </ConfigProvider>
  );
}

export default App;
