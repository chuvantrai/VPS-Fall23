import { Breadcrumb } from 'antd';
import ContentLayout from '@/layouts/components/Content/ContentLayout';
import { Content } from 'antd/es/layout/layout';
import { Link, useLocation } from 'react-router-dom';

function ManageLayout({ isShow, contentItem }) {
  const location = useLocation();
  const pathnames = location.pathname.split('/').filter((x) => x);

  return (
    <Content
      style={{
        // background: colorBgContainer,
        overflow: 'initial',
        height: "100%",
        padding: '1%'
      }}>
      {console.log(pathnames)}
      <Breadcrumb style={{ margin: '16px 0' }}>
        <Breadcrumb.Item>Home</Breadcrumb.Item>
        {pathnames.map((name, index) => {
          const routeTo = `/${pathnames.slice(0, index + 1).join('/')}`;
          const isLast = index === pathnames.length - 1;

          return (
            <Breadcrumb.Item key={routeTo}>
              {isLast ? (
                name
              ) : (
                <Link to={routeTo}>{name}</Link>
              )}
            </Breadcrumb.Item>
          );
        })}
      </Breadcrumb>
      {isShow && <ContentLayout></ContentLayout>}
    </Content>
  );
}
export default ManageLayout;
