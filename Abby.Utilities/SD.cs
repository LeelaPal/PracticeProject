using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.Utilities
{
	public static class SD
	{
		//Order Status
		public const string StatusPending = "Payment Pending";
		public const string StatusSubmitted = "Payment Approved and Order Submitted";
		public const string StatusRejected = "Payment Rejected";
		public const string StatusInProgress = "Order is being prepared";
		public const string StatusReady = "Order is ready for pickup";
		public const string StatusCompleted = "Completed";
		public const string StatusCancelled = "Cancelled";
		public const string StatusRefunded = "Refunded";
	}
}
