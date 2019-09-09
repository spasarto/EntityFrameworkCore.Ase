using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.Ase.Internal
{
    public enum AseValueGenerationStrategy
    {
        /// <summary>
        ///     No SQL Server-specific strategy
        /// </summary>
        None,

        /// <summary>
        ///     <para>
        ///         A sequence-based hi-lo pattern where blocks of IDs are allocated from the server and
        ///         used client-side for generating keys.
        ///     </para>
        ///     <para>
        ///         This is an advanced pattern--only use this strategy if you are certain it is what you need.
        ///     </para>
        /// </summary>
        SequenceHiLo,

        /// <summary>
        ///     A pattern that uses a normal SQL Server <c>Identity</c> column in the same way as EF6 and earlier.
        /// </summary>
        IdentityColumn
    }
}
