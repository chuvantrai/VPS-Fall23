import { Breadcrumb, Layout, Typography, theme } from "antd"
import { Content } from "antd/es/layout/layout"
import ContentLayout from "./components/Content/ContentLayout"
import Sidebar from "./components/Sidebar/Sidebar"
import { useLocation, useNavigate } from "react-router-dom"
import { useMemo } from "react"
import routes from "../routes"
import getAccountJwtModel from "../helpers/getAccountJwtModel"

const ManagerLayout = () => {
    const {
        token: { colorBgContainer },
    } = theme.useToken();

    const location = useLocation();
    const locationSplitted = useMemo(() => {
        return location.pathname.split('/');
    }, [location.pathname])

    const account = getAccountJwtModel();
    const userRoutesConfig = routes.main[account?.RoleId ?? 0];
    const findRouteConfigByPath = (routesConfig, path) => {
        if (!routesConfig || routesConfig.length == 0) {
            return null;
        }
        let routeConfig = routesConfig?.find((route) => route.path == path)
        if (routeConfig) {
            return routeConfig
        }

        return findRouteConfigByPath(routesConfig?.filter((r) => r.children)?.flatMap((r) => r.children), path)
    }
    return (
        <Layout style={{ marginTop: 10 }}>
            <Sidebar />
            <Content
                style={{
                    overflow: 'initial',
                    height: '100%',
                    paddingLeft: 10
                }}
            >
                <Layout style={{ marginBottom: '1%', backgroundColor: colorBgContainer, padding: '1%' }} >
                    <Breadcrumb separator=">"
                        items={locationSplitted.map((path, index) => {
                            let routeConfig = findRouteConfigByPath(userRoutesConfig.routes, path)
                            return { title: routeConfig?.label }

                        })}
                    />
                    <Typography.Title level={4} style={{ margin: 0 }}>
                        {findRouteConfigByPath(userRoutesConfig.routes, locationSplitted[locationSplitted.length - 1])?.description}
                    </Typography.Title>
                </Layout>

                <ContentLayout></ContentLayout>
            </Content>
        </Layout>
    )
}
export default ManagerLayout