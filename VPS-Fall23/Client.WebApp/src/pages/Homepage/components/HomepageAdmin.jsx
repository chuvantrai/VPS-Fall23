import { Layout } from 'antd';
import Sidebar from '../../../layouts/components/Sidebar/Sidebar';
const { Content } = Layout;

function HomepageAdmin() {
    const rowData = ["user", "manager"]
    return (
        <div className='m-auto w-full'>
            <Layout>
                <Content
                    style={{
                        padding: '0 50px',
                    }}
                >
                    <Sidebar rowData={rowData}></Sidebar>
                    {/* <Layout
                        style={{
                            padding: '24px 0',
                            background: colorBgContainer,
                        }}
                    >
                        <Sider
                            style={{
                                background: colorBgContainer,
                            }}
                            width={200}
                        >
                            <Menu
                                mode="inline"
                                defaultSelectedKeys={['1']}
                                defaultOpenKeys={['sub1']}
                                style={{
                                    height: '100%',
                                }}
                                items={items2}
                            />
                        </Sider>
                        <Content
                            style={{
                                padding: '0 24px',
                                minHeight: 280,
                            }}
                        >
                            Content
                        </Content>
                    </Layout> */}
                </Content>
            </Layout>

        </div>
    )
}

export default HomepageAdmin