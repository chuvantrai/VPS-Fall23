import { Layout } from "antd"
import { Outlet } from "react-router-dom"


const DriverLayout = () => {
    return (<Layout style={{ position: 'relative' }}>
        <Outlet></Outlet>
    </Layout>)
}
export default DriverLayout