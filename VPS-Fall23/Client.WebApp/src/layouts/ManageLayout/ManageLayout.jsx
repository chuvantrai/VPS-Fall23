import { Breadcrumb } from 'antd';
import ContentLayout from '@/layouts/components/Content/ContentLayout';
import { Content } from 'antd/es/layout/layout';
import { Link, useLocation } from 'react-router-dom';
import SearchBar from '@/components/searchbar/SearchBar';

// eslint-disable-next-line react/prop-types, no-unused-vars
function ManageLayout({ isShow, contentItem }) {
  const location = useLocation();
  const pathnames = location.pathname.split('/').filter((x) => x);
  const breadcrumbItems = pathnames.map((name, index) => {
    const routeTo = `/${pathnames.slice(0, index + 1).join('/')}`;
    const isLast = index === pathnames.length - 1;

    return {
      path: routeTo,
      breadcrumbName: isLast ? name : <Link to={routeTo}>{name}</Link>,
    };
  });

  return (
    <Content
      style={{
        // background: colorBgContainer,
        overflow: 'initial',
        height: "100%",
        padding: '1%'
      }}>
      <Breadcrumb separator=">">
        <Breadcrumb.Item>Home</Breadcrumb.Item>

        {breadcrumbItems.map((item) => (
          <Breadcrumb.Item key={item.path}>{item.breadcrumbName}</Breadcrumb.Item>
        ))}
      </Breadcrumb>

      {isShow && <ContentLayout></ContentLayout>}
    </Content>
  );
}
export default ManageLayout;
