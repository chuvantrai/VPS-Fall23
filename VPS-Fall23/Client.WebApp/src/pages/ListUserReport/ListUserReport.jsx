/* eslint-disable react-hooks/exhaustive-deps */
import { Table, Button, Pagination, Select, Tooltip, Tag } from 'antd';
import { QuestionCircleOutlined, FilterOutlined } from '@ant-design/icons';
import { useEffect, useRef, useState, Fragment } from 'react';

import useReportServices from '@/services/reportServices';

function ListReport() {
  const service = useReportServices();

  const columns = [
    {
      title: 'Kiểu báo cáo',
      dataIndex: 'typeName',
      key: 'typeName',
    },
    {
      title: 'Người tạo',
      dataIndex: 'email',
      key: 'email',
    },
    {
      title: 'SĐT',
      dataIndex: 'phone',
      key: 'phone',
    },
    {
      title: 'Thời gian tạo',
      dataIndex: 'createdAt',
      key: 'createdAt',
    },
    {
      title: 'Nội dung',
      key: 'content',
      dataIndex: 'content',
      width: '20%',
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      render: (value) => (
        <Fragment>
          <Tag color={getStatusColor(value)}>
            <a>{value}</a>
          </Tag>
        </Fragment>
      ),
    },
    {
      title: 'Thay đổi trạng thái',
      dataIndex: 'status',
      key: 'status',
      render: (value, record) => (
        <Select defaultValue={value} onChange={(newValue) => handleStatusChange(record, newValue)}>
          {getStatusOptions()}
        </Select>
      ),
    },
  ];

  const [data, setData] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [typeOptions, setTypeOptions] = useState([]);
  const [statusOptions, setStatusOptions] = useState([]);
  const [filterType, setFilterType] = useState('');
  const [filterStatus, setFilterStatus] = useState('');
  const staticStatusOptions = [
    { value: 'Hủy', label: 'Hủy', color: 'red', id: 6 },
    { value: 'Hoàn thành', label: 'Hoàn thành', color: 'green', id: 5 },
    { value: 'Đang xử lý', label: 'Đang xử lý', color: 'yellow', id: 4 },
  ];

  const handleChangePage = (page) => {
    setPageNumber(page);
  };

  const loadData = () => {
    service.getReportForAdmin(pageNumber).then((res) => {
      setData(res.data.data);
      setTotalItems(res.data.totalCount);
    });
  };

  const loadTypeOptions = () => {
    service.loadReportType().then((res) => {
      setTypeOptions(res.data);
    });
  };

  const loadStatusOptions = () => {
    setStatusOptions(getStatusOptionsFromStatic());
  };

  const getStatusOptionsFromStatic = () => {
    return staticStatusOptions.map((status) => ({
      value: status.value,
      label: status.label,
    }));
  };

  const firstUpdate = useRef(true);
  useEffect(() => {
    if (firstUpdate.current) {
      firstUpdate.current = false;
      loadData();
      return;
    }
    if ((filterType === undefined || filterType === '') && (filterStatus === undefined || filterStatus === '')) {
      loadData();
    } else {
      handleFilterReport();
    }
  }, [pageNumber]);

  useEffect(() => {
    loadTypeOptions();
    loadStatusOptions();
    getStatusOptions();
  }, []);

  const getStatusColor = (status) => {
    const foundStatus = staticStatusOptions.find((s) => s.label === status);
    return foundStatus ? foundStatus.color : '';
  };

  const getStatusValue = (status) => {
    const foundStatus = staticStatusOptions.find((s) => s.label === status);
    return foundStatus ? foundStatus.id : '';
  };

  const handleFilterReport = () => {
    if ((filterType === undefined || filterType === '') && (filterStatus === undefined || filterStatus === '')) {
      loadData();
    } else {
      const id = getStatusValue(filterStatus) ? getStatusValue(filterStatus) : 0;
      const type = filterType ? filterType : 0;
      service.filterReport(pageNumber, 10, type, id).then((res) => {
        setData(res.data.data);
        setTotalItems(res.data.totalCount);
      });
    }
  };

  const getStatusOptions = () => {
    return staticStatusOptions.map((status) => (
      <Select.Option key={status.value} value={status.value}>
        {status.label}
      </Select.Option>
    ));
  };

  const handleStatusChange = (record, newValue) => {
    const selectedStatus = staticStatusOptions.find((status) => status.value === newValue);
    const selectedId = selectedStatus ? selectedStatus.id : null;

    setFilterType('');
    setFilterStatus('');

    service.updateStatusReport(record.id, selectedId).then(() => {
      loadData();
    });
  };

  const onChangeType = (value) => {
    setFilterType(value);
  };

  const onChangeStatus = (value) => {
    setFilterStatus(value);
  };

  const filterOption = (input, option) => (option?.label ?? '').toLowerCase().includes(input.toLowerCase());

  return (
    <div className="w-[100%]">
      <div className="mb-[16px] flex items-center">
        <p className="mr-[16px]">
          Lọc{' '}
          <Tooltip title="Lọc theo kiểu và trạng thái báo cáo">
            <QuestionCircleOutlined />
          </Tooltip>{' '}
          :
        </p>
        <Select
          className="w-80"
          showSearch
          placeholder="kiểu báo cáo"
          optionFilterProp="children"
          allowClear
          onChange={onChangeType}
          filterOption={filterOption}
          options={typeOptions}
        />

        <Select
          className="w-80 ml-[10px]"
          showSearch
          placeholder="trạng thái báo cáo"
          optionFilterProp="children"
          allowClear
          onChange={onChangeStatus}
          filterOption={filterOption}
          options={statusOptions}
        />

        <Button className="ml-[10px]" type="primary" icon={<FilterOutlined />} onClick={handleFilterReport}>
          Áp dụng
        </Button>
      </div>

      <Table columns={columns} dataSource={data} pagination={false} />
      <div className="py-[16px] flex flex-row-reverse pr-[24px]">
        <Pagination current={pageNumber} onChange={handleChangePage} total={totalItems} showSizeChanger={false} />
      </div>
    </div>
  );
}

export default ListReport;
