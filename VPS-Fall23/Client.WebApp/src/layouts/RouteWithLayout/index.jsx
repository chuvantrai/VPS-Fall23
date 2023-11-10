import { Route, Routes, useLocation } from "react-router-dom"
import routes from "@/routes"
import guidGenerator from "@/helpers/guidGenerator"
import getAccountJwtModel from "@/helpers/getAccountJwtModel"
import classNames from 'classnames/bind';
import styles from './index.module.scss'
import { Layout } from "antd";
const cx = classNames.bind(styles);
const RouteWithLayout = () => {
    const account = getAccountJwtModel();
    const userRoutesConfig = routes.main[account?.RoleId ?? 0]
    const location = useLocation();


    const getRouteComponent = (parentRoutes, parentPath) => {
        return parentRoutes.map((parentRoute, index) => {

            return (parentRoute.component ?
                <Route
                    path={parentPath ? `${parentPath}/${parentRoute.path}` : parentRoute.path}
                    Component={parentRoute.component}
                    key={guidGenerator()}
                >
                    {
                        parentRoute.children ? getRouteComponent(parentRoute.children) : <></>
                    }
                </Route >
                :
                <Route key={guidGenerator()}>
                    {
                        parentRoute.children ? getRouteComponent(parentRoute.children, parentRoute.path) : <></>
                    }
                </Route>)
        })
    }
    return routes.noLayout.every((route) => route.path !== location.pathname) && (<Layout className={cx('wrapper w-full min-h-screen')}>
        {
            userRoutesConfig.header !== null ? <userRoutesConfig.header /> : <></>
        }
        <Routes>
            <Route key={guidGenerator()} path='/' Component={userRoutesConfig.layout} >
                {
                    getRouteComponent(userRoutesConfig.routes)
                }
            </Route>
        </Routes>
        {
            userRoutesConfig.footer != null ? <userRoutesConfig.footer /> : <></>
        }
    </Layout>
    )


}
export default RouteWithLayout;