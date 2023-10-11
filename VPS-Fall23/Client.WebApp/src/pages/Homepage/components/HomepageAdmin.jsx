import Sidebar from '../../../layouts/components/Sidebar/Sidebar';
import UserProfile from './Content/UserProfile';
import { theme, Layout } from 'antd';
import { useState } from 'react';
import ViewListParkingZone from './Content/ViewListParkingZone';
const { Content } = Layout;

function HomepageAdmin() {
    const {
        token: { colorBgContainer },
    } = theme.useToken();

    const [contentState, setContentState] = useState("1");
    const rowData = ["user", "manager"]

    return (
        <div className='m-auto w-full h-full mt-10'>
            <Layout>
                <Content
                >
                    <Layout
                        style={{
                            padding: '24px 0',
                            background: colorBgContainer,
                        }}
                    >
                        <Sidebar rowData={rowData} setContentState={setContentState}></Sidebar>
                        <Content
                            style={{
                                padding: '0 24px',
                                minHeight: 280,
                            }}
                        >
                            {contentState === "1" && <UserProfile></UserProfile>}
                            {contentState === "2" && <ViewListParkingZone></ViewListParkingZone>}
                        </Content>
                    </Layout>
                </Content>
            </Layout>

        </div >
    )
}

export default HomepageAdmin