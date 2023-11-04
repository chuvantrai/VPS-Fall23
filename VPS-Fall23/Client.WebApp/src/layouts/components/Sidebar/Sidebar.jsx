import { useLocation, useNavigate } from 'react-router-dom';
import PropTypes from 'prop-types';
import { Layout, Menu, theme } from 'antd';
import { useEffect, useState } from 'react';
import getAccountJwtModel from '../../../helpers/getAccountJwtModel';
import routes from '../../../routes';

const { Sider } = Layout;
const defaultSelectedKeys = '/'
function Sidebar() {
  const navigate = useNavigate();
  const location = useLocation();
  const { token: { colorBgContainer }, } = theme.useToken();
  const [menuItems, setMenuItems] = useState([])
  const [selectedKeys, setSelectedKey] = useState();
  const account = getAccountJwtModel();
  let routesConfig = routes.main[account?.RoleId ?? 0]
  useEffect(() => {
    setMenuItems(routesConfig.routes)
    const path = location.pathname.split('/');
    path.splice(0, 1);
    setSelectedKey(findRouteConfigByPaths(routesConfig.routes, path) ?? defaultSelectedKeys)
  }, [])
  const findRouteConfigByPaths = (routesConfigParam, paths) => {
    let i = 0;
    let routeConfig = routesConfigParam.find((r) => r.path == paths[i])
    let routeKey = routeConfig?.key
    if (paths.length < 2) return routeKey;
    while (i < paths.length) {
      routesConfigParam = routeConfig?.children
      routeKey = routesConfigParam.find((r) => r.path == paths[i])?.key
      i++;
    }
    return routeKey
  }
  const findRouteConfig = (routesConfigParam, key) => {
    const routeConfig = routesConfigParam.find((r) => r.key === key)
    if (routeConfig) {
      return routeConfig;
    }
    var flat = routesConfigParam.filter((r) => r.children).map((r) => r.children).flat()
    if (!flat || flat.length == 0) {
      return null;
    }
    return findRouteConfig(flat, key);
  }

  const handleMenuItem = (e) => {
    setSelectedKey(e.keyPath[0]);
    let keyPathReverse = e.keyPath.reverse();

    let keyPaths = keyPathReverse.map((k) => { return findRouteConfig(routesConfig.routes, k)?.path })

    navigate(keyPaths.join('/'));
  };
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
        defaultSelectedKeys={defaultSelectedKeys}
        selectedKeys={selectedKeys}
        items={menuItems}
      />
    </Sider>
  );
}

export default Sidebar;
