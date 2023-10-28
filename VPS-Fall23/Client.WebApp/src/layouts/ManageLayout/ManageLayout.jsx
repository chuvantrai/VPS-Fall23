import { Breadcrumb } from 'antd';
import PropTypes from 'prop-types';

import ContentLayout from '@/layouts/components/Content/ContentLayout';
import { Content } from 'antd/es/layout/layout';
import { Link, useLocation } from 'react-router-dom';

// eslint-disable-next-line react/prop-types, no-unused-vars
function ManageLayout({ isShow, contentItem }) {
  const location = useLocation();
  const pathnames = location.pathname.split('/').filter((x) => x);
  const breadcrumbItems = pathnames.map((name, index) => {
    const routeTo = `/${pathnames.slice(0, index + 1).join('/')}`;
    const isLast = index === pathnames.length - 1;
    name = capitalizeFirstLetter(name);
    return {
      path: routeTo,
      breadcrumbName: isLast ? name : <Link to={routeTo}>{name}</Link>,
    };
  });

  function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
  }

  return (
    <Content
      style={{
        // background: colorBgContainer,
        overflow: 'initial',
        height: '100%',
        padding: '1%',
      }}
    >
      <Breadcrumb separator=">">
        <Breadcrumb.Item>Home</Breadcrumb.Item>

        {breadcrumbItems.map((item) => (
          <Breadcrumb.Item key={item.path}>{item.breadcrumbName}</Breadcrumb.Item>
        ))}
      </Breadcrumb>
      {isShow && <ContentLayout title={contentItem.title} desc={contentItem.desc}></ContentLayout>}
    </Content>
  );
}

ManageLayout.propTypes = {
  contentItem: PropTypes.object,
};

export default ManageLayout;
