import { Layout, theme } from 'antd';
import PropTypes from 'prop-types';
import { Outlet } from 'react-router-dom';

const { Content } = Layout;

function ContentLayout() {
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  return (
    <Content
      className="flex justify-center items-center"
      style={{
        backgroundColor: colorBgContainer,
        margin: 0,
      }}
    >
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
