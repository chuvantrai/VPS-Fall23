import { useNavigate } from 'react-router-dom';
import PropTypes from 'prop-types';
import { Layout, Menu, theme } from 'antd';

const { Sider } = Layout;

function Sidebar({ rowData, setSelectedKey }) {
  const navigate = useNavigate();
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  const handleMenuItem = (e) => {
    setSelectedKey({ label: e.keyPath[1], url: e.keyPath[0] });
    navigate(e.key);
  };

  const items = rowData.map(({ label, options }) => {
    return {
      key: `${label}`,
      // icon: React.createElement(icon),
      label: `${label}`,
      children: options.map(({ label, url }) => {
        return {
          key: `${url}`,
          label: `${label}`,
        };
      }),
    };
  });

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
        defaultSelectedKeys={['1']}
        defaultOpenKeys={['sub1']}
        items={items}
      />
    </Sider>
  );
}

Sidebar.propTypes = {
  rowData: PropTypes.arrayOf(
    PropTypes.shape({
      lable: PropTypes.string,
      options: PropTypes.arrayOf(
        PropTypes.shape({
          label: PropTypes.string,
          url: PropTypes.string,
        }),
      ),
    }),
  ),

  setSelectedKey: PropTypes.func,
};

export default Sidebar;
