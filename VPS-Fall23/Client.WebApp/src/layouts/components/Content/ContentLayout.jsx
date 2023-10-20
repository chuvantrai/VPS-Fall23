import { Layout, theme } from 'antd';
import PropTypes from 'prop-types';
import { Outlet } from 'react-router-dom';

const { Content } = Layout;

function ContentLayout() {
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  return (
    <Content style={{
      backgroundColor: colorBgContainer,
    }}>

      <Outlet></Outlet>
    </Content>
  );
}

ContentLayout.propTypes = {
  contentState: PropTypes.string,
  title: PropTypes.string,
  desc: PropTypes.string,
};

export default ContentLayout;
