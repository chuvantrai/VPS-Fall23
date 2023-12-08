import { Button, Typography } from "antd"
import { Link } from "react-router-dom"

const { Text } = Typography;
const DriverRightHeader = () => {
    return (
        <Text>
            Quản lý nhà xe của bạn?
            <Link to='/login'>
                <Button
                    type='primary'
                    htmlType='button'
                    style={{
                        backgroundColor: '#1677ff',
                        marginLeft: '10px',
                    }}
                >
                    Đăng nhập
                </Button>
            </Link>
        </Text>
    )
}
export default DriverRightHeader