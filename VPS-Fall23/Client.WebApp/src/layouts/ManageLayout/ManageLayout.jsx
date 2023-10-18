import { Breadcrumb } from 'antd';
import ContentLayout from '@/layouts/components/Content/ContentLayout';
import { Content } from 'antd/es/layout/layout';

function ManageLayout({ isShow, contentItem }) {
  return (
    <Content
      style={{
        // background: colorBgContainer,
        overflow: 'initial',
        height: "100%",
        padding: '1%'
      }}>
      <Breadcrumb>
        <Breadcrumb.Item>{contentItem.title}</Breadcrumb.Item>
        <Breadcrumb.Item>{contentItem.desc}</Breadcrumb.Item>
      </Breadcrumb>
      {isShow && <ContentLayout></ContentLayout>}
    </Content>
  );
}
export default ManageLayout;
